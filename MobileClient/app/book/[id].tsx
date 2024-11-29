import { useCallback, useEffect, useState } from "react";
import { StyleSheet, Alert, ScrollView } from "react-native";
import { useFocusEffect, useLocalSearchParams, useRouter } from "expo-router";
import { Audio } from "expo-av";
import {
  Button,
  Card,
  Title,
  Paragraph,
  List,
  Surface,
  IconButton,
  useTheme,
  ActivityIndicator,
  Divider,
  Avatar,
} from "react-native-paper";

import { ThemedView } from "@/components/ThemedView";
import { getBookById } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book } from "@/services/books";

export default function BookScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useUserStore((state) => state.user);
  const [sound, setSound] = useState<Audio.Sound | null>(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentAudioId, setCurrentAudioId] = useState<number | null>(null);

  useEffect(() => {
    return sound
      ? () => {
          sound.unloadAsync();
        }
      : undefined;
  }, [sound]);

  async function handlePlayAudio(audioUrl: string, audioId: number) {
    try {
      if (sound) {
        await sound.unloadAsync();
        setSound(null);
        setIsPlaying(false);

        if (currentAudioId === audioId) {
          setCurrentAudioId(null);
          return;
        }
      }

      const { sound: newSound } = await Audio.Sound.createAsync(
        { uri: audioUrl },
        { shouldPlay: true }
      );

      setSound(newSound);
      setIsPlaying(true);
      setCurrentAudioId(audioId);

      newSound.setOnPlaybackStatusUpdate((status) => {
        if (status.isLoaded && status.didJustFinish) {
          setIsPlaying(false);
          setCurrentAudioId(null);
        }
      });
    } catch (error) {
      console.error("Failed to play audio:", error);
      Alert.alert("Error", "Failed to play audio");
    }
  }

  async function handlePauseAudio() {
    if (sound) {
      await sound.pauseAsync();
      setIsPlaying(false);
    }
  }

  async function handleResumeAudio() {
    if (sound) {
      await sound.playAsync();
      setIsPlaying(true);
    }
  }

  async function fetchBook() {
    try {
      const fetchedBook = await getBookById(Number(id));
      setBook(fetchedBook);
      console.log(fetchedBook);
    } catch (error) {
      console.error("Failed to fetch book:", error);
      Alert.alert("Error", "Failed to load book details");
      router.back();
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchBook();
  }, [id]);

  useFocusEffect(
    useCallback(() => {
      fetchBook();
    }, [])
  );

  const theme = useTheme();

  if (loading || !book) {
    return (
      <ThemedView style={styles.loadingContainer}>
        <ActivityIndicator size="large" />
      </ThemedView>
    );
  }

  const isOwner = book.author.id === user?.id;

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content}>
        <Card style={styles.headerCard}>
          <Surface
            style={[
              styles.headerContent,
              { backgroundColor: theme.colors.surfaceVariant },
            ]}
            elevation={2}
          >
            <Card elevation={4}>
              <Card.Cover
                source={{ uri: book.metadata.image_url }}
                style={styles.coverImage}
                resizeMode="cover"
              />
            </Card>
            <Surface
              style={[
                styles.bookInfo,
                { backgroundColor: theme.colors.surfaceVariant },
              ]}
              elevation={0}
            >
              <Title
                style={[styles.title, { color: theme.colors.onSurfaceVariant }]}
              >
                {book.metadata.title}
              </Title>
              <Surface
                style={[
                  styles.authorRow,
                  { backgroundColor: theme.colors.surfaceVariant },
                ]}
                elevation={0}
              >
                <Avatar.Icon
                  icon="account"
                  size={24}
                  style={styles.authorIcon}
                  color={theme.colors.primary}
                />
                <Paragraph
                  style={[
                    styles.subtitle,
                    { color: theme.colors.onSurfaceVariant },
                  ]}
                >
                  By {book.author.username}
                </Paragraph>
              </Surface>
              <Paragraph
                style={[
                  styles.description,
                  { color: theme.colors.onSurfaceVariant },
                ]}
              >
                {book.metadata.description}
              </Paragraph>
            </Surface>
          </Surface>
        </Card>

        <Card style={styles.section}>
          <Card.Content>
            <Surface
              style={[
                styles.sectionHeader,
                { backgroundColor: theme.colors.surfaceVariant },
              ]}
              elevation={1}
            >
              <Avatar.Icon
                icon="book-open-variant"
                size={24}
                style={styles.sectionIcon}
                color={theme.colors.primary}
              />
              <Title style={{ color: theme.colors.onSurfaceVariant }}>
                Chapters
              </Title>
            </Surface>
            {book.metadata.chapters.length === 0 ? (
              <Paragraph
                style={[
                  styles.placeholderText,
                  { color: theme.colors.onSurfaceVariant },
                ]}
              >
                No chapters available
              </Paragraph>
            ) : (
              book.metadata.chapters.map((chapter) => (
                <Card
                  key={chapter.index}
                  style={[
                    styles.chapterCard,
                    { backgroundColor: theme.colors.surfaceVariant },
                  ]}
                >
                  <Card.Content>
                    <Surface
                      style={[
                        styles.chapterHeader,
                        { backgroundColor: theme.colors.surfaceVariant },
                      ]}
                      elevation={0}
                    >
                      <Avatar.Icon
                        icon="bookmark"
                        size={24}
                        style={styles.chapterIcon}
                        color={theme.colors.primary}
                      />
                      <Title
                        style={[
                          styles.chapterTitle,
                          { color: theme.colors.onSurfaceVariant },
                        ]}
                      >
                        Chapter {chapter.index + 1}: {chapter.title}
                      </Title>
                    </Surface>
                    <Paragraph
                      style={[
                        styles.chapterContent,
                        { color: theme.colors.onSurfaceVariant },
                      ]}
                    >
                      {chapter.content}
                    </Paragraph>
                  </Card.Content>
                </Card>
              ))
            )}
          </Card.Content>
        </Card>

        <Card style={styles.section}>
          <Card.Content>
            <Surface
              style={[
                styles.sectionHeader,
                { backgroundColor: theme.colors.surfaceVariant },
              ]}
              elevation={1}
            >
              <Avatar.Icon
                icon="headphones"
                size={24}
                style={styles.sectionIcon}
                color={theme.colors.primary}
              />
              <Title style={{ color: theme.colors.onSurfaceVariant }}>
                Audio Books
              </Title>
            </Surface>
            {!book.audio_books || book.audio_books.length === 0 ? (
              <Paragraph
                style={[
                  styles.placeholderText,
                  { color: theme.colors.onSurfaceVariant },
                ]}
              >
                No audio books available
              </Paragraph>
            ) : (
              book.audio_books.map((audio) => (
                <Surface
                  key={audio.id}
                  style={[
                    styles.audioBook,
                    { backgroundColor: theme.colors.surfaceVariant },
                  ]}
                  elevation={2}
                >
                  <Surface
                    style={[
                      styles.audioInfo,
                      { backgroundColor: theme.colors.surfaceVariant },
                    ]}
                    elevation={0}
                  >
                    <Avatar.Icon
                      icon="music"
                      size={24}
                      style={styles.audioIcon}
                      color={theme.colors.primary}
                    />
                    <Title
                      style={[
                        styles.audioTitle,
                        { color: theme.colors.onSurfaceVariant },
                      ]}
                    >
                      Audio Book {audio.id}
                    </Title>
                  </Surface>
                  <IconButton
                    icon={
                      currentAudioId === audio.id
                        ? isPlaying
                          ? "pause"
                          : "play"
                        : "play"
                    }
                    mode="contained"
                    onPress={() => {
                      if (currentAudioId === audio.id) {
                        isPlaying ? handlePauseAudio() : handleResumeAudio();
                      } else {
                        handlePlayAudio(audio.audio_url, audio.id);
                      }
                    }}
                  />
                </Surface>
              ))
            )}
          </Card.Content>
        </Card>

        {isOwner && (
          <Surface style={styles.actions} elevation={0}>
            <Button
              mode="contained"
              icon="pencil"
              onPress={() =>
                router.push({
                  pathname: "/book/edit/[id]",
                  params: { id: book.id },
                })
              }
              style={styles.actionButton}
            >
              Edit Book
            </Button>
            <Button
              mode="contained"
              icon="delete"
              buttonColor={theme.colors.error}
              onPress={() =>
                router.push({
                  pathname: "/book/delete/[id]",
                  params: { id: book.id },
                })
              }
              style={styles.actionButton}
            >
              Delete Book
            </Button>
          </Surface>
        )}
      </Surface>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  content: {
    padding: 16,
    gap: 16,
    paddingBottom: 32,
  },
  headerCard: {
    overflow: "hidden",
    borderRadius: 12,
  },
  headerContent: {
    flexDirection: "row",
    padding: 16,
    gap: 16,
    borderRadius: 12,
  },
  coverImage: {
    width: 120,
    height: 180,
    borderRadius: 8,
  },
  bookInfo: {
    flex: 1,
    gap: 8,
  },
  authorRow: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
  },
  authorIcon: {
    backgroundColor: "transparent",
  },
  title: {
    fontSize: 24,
    fontWeight: "bold",
  },
  subtitle: {
    fontSize: 16,
    opacity: 0.7,
  },
  description: {
    fontSize: 14,
    opacity: 0.8,
  },
  section: {
    marginVertical: 8,
  },
  sectionHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginBottom: 16,
    padding: 12,
    borderRadius: 8,
  },
  sectionIcon: {
    backgroundColor: "transparent",
  },
  chapterCard: {
    marginBottom: 16,
    borderRadius: 8,
  },
  chapterHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginBottom: 8,
  },
  chapterIcon: {
    backgroundColor: "transparent",
  },
  chapterTitle: {
    fontSize: 18,
    fontWeight: "600",
  },
  chapterContent: {
    fontSize: 14,
    lineHeight: 20,
  },
  audioBook: {
    padding: 16,
    marginVertical: 8,
    borderRadius: 12,
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "space-between",
  },
  audioInfo: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
  },
  audioIcon: {
    backgroundColor: "transparent",
  },
  audioTitle: {
    fontSize: 16,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 16,
  },
  actionButton: {
    flex: 1,
  },
  placeholderText: {
    fontStyle: "italic",
    textAlign: "center",
    opacity: 0.7,
  },
});
