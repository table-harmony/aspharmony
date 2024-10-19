using System.Drawing;
using System.Text;

namespace Utils.Encryption {
    /// <summary>
    /// Provides methods for encoding and decoding text in images using steganography.
    /// </summary>
    public static class Steganography {
        // State to indicate whether we're hiding the text or filling with zeros (end of data).
        private enum State {
            Hiding,
            FillingWithZeros
        }

        /// <summary>
        /// Encodes the given text into the bitmap image by modifying its pixels' least significant bits (LSB).
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="bmp">The image where the text will be encoded.</param>
        /// <returns>The bitmap with the hidden text.</returns>
        public static Bitmap Encode(string text, Bitmap bmp) {
            State state = State.Hiding;
            int charIndex = 0;
            int charValue = 0;
            long pixelElementIndex = 0;
            int zerosAdded = 0;
            int red = 0, green = 0, blue = 0;

            for (int i = 0; i < bmp.Height; i++) {
                for (int j = 0; j < bmp.Width; j++) {
                    Color pixel = bmp.GetPixel(j, i);

                    // Clear the least significant bit (LSB) of each color channel
                    red = pixel.R & ~1;
                    green = pixel.G & ~1;
                    blue = pixel.B & ~1;

                    for (int n = 0; n < 3; n++) {
                        // Process 8 bits of text (1 byte) at a time
                        if (pixelElementIndex % 8 == 0) {
                            if (state == State.FillingWithZeros && zerosAdded == 8) {
                                if ((pixelElementIndex - 1) % 3 < 2)
                                    bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));

                                return bmp;
                            }

                            if (charIndex >= text.Length)
                                state = State.FillingWithZeros;  // Finished encoding, now fill with zeros
                            else
                                charValue = text[charIndex++];  // Get next character to encode
                        }

                        // Set LSB of the current color channel
                        switch (pixelElementIndex % 3) {
                            case 0:
                                red = EmbedBitIntoColor(red, ref charValue, state);
                                break;
                            case 1:
                                green = EmbedBitIntoColor(green, ref charValue, state);
                                break;
                            case 2:
                                blue = EmbedBitIntoColor(blue, ref charValue, state);
                                bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.FillingWithZeros)
                            zerosAdded++;  // Fill with zeros until we reach 8 bits
                    }
                }
            }

            return bmp;
        }

        /// <summary>
        /// Decodes hidden text from the bitmap image by reading the least significant bits (LSB) of each pixel.
        /// </summary>
        /// <param name="bmp">The image containing hidden text.</param>
        /// <returns>The extracted hidden text.</returns>
        public static string Decode(Bitmap bmp) {
            int colorUnitIndex = 0;
            int charValue = 0;
            StringBuilder extractedText = new();

            for (int i = 0; i < bmp.Height; i++) {
                for (int j = 0; j < bmp.Width; j++) {
                    Color pixel = bmp.GetPixel(j, i);

                    for (int n = 0; n < 3; n++) {
                        switch (colorUnitIndex % 3) {
                            case 0:
                                charValue = ExtractBitFromColor(charValue, pixel.R);
                                break;
                            case 1:
                                charValue = ExtractBitFromColor(charValue, pixel.G);
                                break;
                            case 2:
                                charValue = ExtractBitFromColor(charValue, pixel.B);
                                break;
                        }

                        colorUnitIndex++;

                        if (colorUnitIndex % 8 == 0)  // When we have a full byte (8 bits)
                        {
                            charValue = ReverseBits(charValue);

                            if (charValue == 0)  // Check for stop character (8 zeros)
                                return extractedText.ToString();

                            extractedText.Append((char)charValue);
                        }
                    }
                }
            }

            return extractedText.ToString();
        }

        /// <summary>
        /// Embeds a bit of the current character into the least significant bit (LSB) of the color channel.
        /// </summary>
        /// <param name="colorValue">The color channel value (red, green, or blue).</param>
        /// <param name="charValue">The character being encoded.</param>
        /// <param name="state">The current state (Hiding or FillingWithZeros).</param>
        /// <returns>The modified color channel value.</returns>
        private static int EmbedBitIntoColor(int colorValue, ref int charValue, State state) {
            if (state == State.Hiding) {
                colorValue += charValue % 2;  // Embed the rightmost bit of the char into the color's LSB
                charValue /= 2;  // Move to the next bit of the character
            }
            return colorValue;
        }

        /// <summary>
        /// Extracts the least significant bit (LSB) from the color channel and adds it to the current character value.
        /// </summary>
        /// <param name="charValue">The current character value being decoded.</param>
        /// <param name="colorValue">The color channel value (red, green, or blue).</param>
        /// <returns>The updated character value.</returns>
        private static int ExtractBitFromColor(int charValue, int colorValue) {
            return charValue * 2 + colorValue % 2;  // Extract the LSB from the color and add it to the character
        }

        /// <summary>
        /// Reverses the bits of the given byte (used for decoding).
        /// </summary>
        /// <param name="n">The byte whose bits are to be reversed.</param>
        /// <returns>The byte with reversed bits.</returns>
        private static int ReverseBits(int n) {
            int result = 0;
            for (int i = 0; i < 8; i++) {
                result = result * 2 + n % 2;
                n /= 2;
            }
            return result;
        }
    }
}
