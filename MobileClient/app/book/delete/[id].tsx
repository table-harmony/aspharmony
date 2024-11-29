import { useState, useEffect } from "react";
import { StyleSheet, Alert } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import {
  Surface,
  Text,
  Button,
  Card,
  Portal,
  Dialog,
  useTheme,
  Avatar,
} from "react-native-paper";

import { deleteBook, getBookById } from "@/services/books";
import type { Book } from "@/services/books";

export default function DeleteBookScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const theme = useTheme();

  useEffect(() => {
    async function fetchBook() {
      try {
        const fetchedBook = await getBookById(Number(id));
        setBook(fetchedBook);
      } catch (error) {
        console.error("Failed to fetch book:", error);
        Alert.alert("Error", "Failed to load book details");
        router.back();
      }
    }
    fetchBook();
  }, [id]);

  async function handleDelete() {
    try {
      await deleteBook(Number(id));
      router.replace("/book");
    } catch (error) {
      console.error("Failed to delete book:", error);
      Alert.alert("Error", "Failed to delete book");
    }
  }

  if (!book) {
    return null;
  }

  return (
    <Surface style={styles.container}>
      <Card style={styles.card}>
        <Card.Cover
          source={{ uri: book.metadata.image_url }}
          style={styles.coverImage}
          resizeMode="contain"
        />
        <Card.Content style={styles.content}>
          <Avatar.Icon
            icon="alert"
            size={64}
            style={styles.warningIcon}
            color={theme.colors.error}
          />
          <Text variant="headlineSmall" style={styles.title}>
            Delete Book
          </Text>
          <Text variant="titleMedium" style={styles.bookTitle}>
            "{book.metadata.title}"
          </Text>
          <Text variant="bodyLarge" style={styles.description}>
            Are you sure you want to delete this book? This action cannot be
            undone.
          </Text>
          <Surface style={styles.actions} elevation={0}>
            <Button
              mode="contained-tonal"
              onPress={() => router.back()}
              style={[styles.button, styles.cancelButton]}
              contentStyle={styles.buttonContent}
            >
              Cancel
            </Button>
            <Button
              mode="contained"
              onPress={handleDelete}
              style={[styles.button, styles.deleteButton]}
              buttonColor={theme.colors.error}
              contentStyle={styles.buttonContent}
            >
              Delete Book
            </Button>
          </Surface>
        </Card.Content>
      </Card>
    </Surface>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    justifyContent: "center",
    paddingBottom: 32,
  },
  card: {
    overflow: "hidden",
  },
  coverImage: {
    height: 250,
    backgroundColor: "#f5f5f5",
    opacity: 0.5,
  },
  content: {
    padding: 24,
    alignItems: "center",
    gap: 24,
  },
  warningIcon: {
    backgroundColor: "transparent",
    marginTop: -32,
    marginBottom: 8,
  },
  title: {
    fontWeight: "bold",
  },
  bookTitle: {
    textAlign: "center",
    fontStyle: "italic",
  },
  description: {
    textAlign: "center",
    opacity: 0.7,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 8,
    width: "100%",
  },
  button: {
    flex: 1,
  },
  buttonContent: {
    paddingVertical: 8,
  },
  cancelButton: {
    borderWidth: 0,
  },
  deleteButton: {
    borderWidth: 0,
  },
});
