import { useEffect, useState } from "react";
import { Alert, Image, ScrollView, StyleSheet } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import {
  Button,
  Card,
  Surface,
  TextInput,
  Portal,
  Dialog,
} from "react-native-paper";

import { ThemedText } from "@/components/ThemedText";
import { LoadingScreen } from "@/components/LoadingScreen";
import { ErrorScreen } from "@/components/ErrorScreen";
import { getBooks } from "@/services/books";
import { getLibrary, addBookToLibrary } from "@/services/libraries";
import type { Book } from "@/services/books";
import type { Library } from "@/services/libraries";

export default function AddBooksScreen() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const libraryId = parseInt(id);
  const router = useRouter();

  const [library, setLibrary] = useState<Library | null>(null);
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [copies, setCopies] = useState("1");
  const [dialogVisible, setDialogVisible] = useState(false);

  const loadData = async () => {
    try {
      setLoading(true);
      const [libraryData, booksData] = await Promise.all([
        getLibrary(libraryId),
        getBooks(),
      ]);
      setLibrary(libraryData);
      setBooks(booksData);
    } catch (err) {
      console.error("Failed to load data:", err);
      setError("Failed to load data");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, [libraryId]);

  const filteredBooks = books.filter(
    (book) =>
      book.metadata.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.author.username.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const handleAddBook = async () => {
    if (!selectedBook || !library) return;

    try {
      const numCopies = parseInt(copies);
      if (isNaN(numCopies) || numCopies < 1 || numCopies > 100) {
        Alert.alert("Error", "Number of copies must be between 1 and 100");
        return;
      }

      for (let i = 0; i < numCopies; i++) {
        await addBookToLibrary(libraryId, selectedBook.id);
      }

      Alert.alert("Success", "Book(s) added to library successfully");
      setDialogVisible(false);
      router.back();
    } catch (err) {
      console.error("Failed to add book:", err);
      Alert.alert("Error", "Failed to add book to library");
    }
  };

  if (loading) return <LoadingScreen />;
  if (error) return <ErrorScreen message={error} onRetry={loadData} />;
  if (!library) return null;

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <Card style={styles.headerCard}>
          <Card.Content>
            <ThemedText type="title">Add Books</ThemedText>
            <ThemedText style={styles.subtitle}>
              Select books to add to {library.name}.{" "}
              {library.allow_copies &&
                "Multiple copies of the same book can be added."}
            </ThemedText>
          </Card.Content>
        </Card>

        <Card style={styles.searchCard}>
          <Card.Content>
            <TextInput
              mode="outlined"
              placeholder="Search books..."
              value={searchQuery}
              onChangeText={setSearchQuery}
              left={<TextInput.Icon icon="magnify" />}
            />
          </Card.Content>
        </Card>

        {filteredBooks.map((book) => (
          <Card key={book.id} style={styles.bookCard}>
            <Card.Content style={styles.bookContent}>
              <Image
                style={styles.bookImage}
                source={{ uri: book.metadata.image_url }}
                defaultSource={require("@/assets/images/placeholder-book.png")}
              />
              <Surface style={styles.bookInfo} elevation={0}>
                <ThemedText type="defaultSemiBold">
                  {book.metadata.title}
                </ThemedText>
                <ThemedText>by {book.author.username}</ThemedText>
                {book.metadata.description && (
                  <ThemedText numberOfLines={2} style={styles.description}>
                    {book.metadata.description}
                  </ThemedText>
                )}
              </Surface>
              <Button
                mode="contained"
                onPress={() => {
                  setSelectedBook(book);
                  setCopies("1");
                  setDialogVisible(true);
                }}
              >
                Add
              </Button>
            </Card.Content>
          </Card>
        ))}

        <Portal>
          <Dialog
            visible={dialogVisible}
            onDismiss={() => setDialogVisible(false)}
          >
            <Dialog.Title>Add Book to Library</Dialog.Title>
            <Dialog.Content>
              <ThemedText>
                Add "{selectedBook?.metadata.title}" to {library.name}?
              </ThemedText>
              {library.allow_copies && (
                <TextInput
                  mode="outlined"
                  label="Number of Copies"
                  keyboardType="numeric"
                  value={copies}
                  onChangeText={setCopies}
                  style={styles.copiesInput}
                />
              )}
            </Dialog.Content>
            <Dialog.Actions>
              <Button onPress={() => setDialogVisible(false)}>Cancel</Button>
              <Button
                mode="contained"
                style={{ paddingHorizontal: 16 }}
                onPress={handleAddBook}
              >
                Add
              </Button>
            </Dialog.Actions>
          </Dialog>
        </Portal>
      </Surface>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  content: {
    padding: 16,
  },
  searchCard: {
    marginBottom: 16,
  },
  bookCard: {
    marginBottom: 8,
  },
  bookContent: {
    flexDirection: "row",
    alignItems: "center",
    gap: 12,
  },
  bookInfo: {
    flex: 1,
  },
  description: {
    marginTop: 4,
    opacity: 0.7,
  },
  copiesInput: {
    marginTop: 16,
  },
  headerCard: {
    marginBottom: 16,
  },
  subtitle: {
    marginTop: 8,
    opacity: 0.7,
  },
  bookImage: {
    width: 80,
    height: 120,
    borderRadius: 4,
  },
});
