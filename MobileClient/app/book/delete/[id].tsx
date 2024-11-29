import { useEffect, useState } from "react";
import { StyleSheet, Alert, Pressable } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { getBookById, deleteBook } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book } from "@/services/books";

export default function DeleteBookScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useUserStore((state) => state.user);

  async function fetchBook() {
    try {
      const fetchedBook = await getBookById(Number(id));
      if (fetchedBook.author.id !== user?.id) {
        Alert.alert("Error", "You don't have permission to delete this book");
        router.back();
        return;
      }
      setBook(fetchedBook);
    } catch (error) {
      console.error("Failed to fetch book:", error);
      Alert.alert("Error", "Failed to load book details");
      router.back();
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete() {
    if (!book) return;

    try {
      await deleteBook(book.id);
      router.replace("/book");
    } catch (error) {
      console.error("Failed to delete book:", error);
      Alert.alert("Error", "Failed to delete book");
    }
  }

  useEffect(() => {
    fetchBook();
  }, [id]);

  if (loading || !book) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading book details...</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.content}>
        <ThemedText type="title">Delete Book</ThemedText>
        <ThemedText type="subtitle">
          Are you sure you want to delete "{book.metadata.title}"?
        </ThemedText>
        <ThemedText style={styles.warning}>
          This action cannot be undone.
        </ThemedText>

        <ThemedView style={styles.actions}>
          <Pressable style={styles.cancelButton} onPress={() => router.back()}>
            <ThemedText style={styles.buttonText}>Cancel</ThemedText>
          </Pressable>
          <Pressable style={styles.deleteButton} onPress={handleDelete}>
            <ThemedText style={styles.buttonText}>Delete Book</ThemedText>
          </Pressable>
        </ThemedView>
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  content: {
    padding: 16,
    gap: 16,
    alignItems: "center",
  },
  warning: {
    color: "#dc3545",
    fontWeight: "600",
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 24,
  },
  cancelButton: {
    flex: 1,
    backgroundColor: "#6c757d",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  deleteButton: {
    flex: 1,
    backgroundColor: "#dc3545",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontWeight: "600",
  },
});
