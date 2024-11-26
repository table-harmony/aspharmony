import { useEffect, useState } from "react";
import { StyleSheet, ScrollView, ActivityIndicator, Alert } from "react-native";
import { useLocalSearchParams, router } from "expo-router";

import { ThemedView } from "@/components/ThemedView";
import { ThemedText } from "@/components/ThemedText";
import { Button } from "@/components/ui/Button";
import { Book, BooksService } from "@/services/books";

export default function BookDetailsScreen() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadBook();
  }, [id]);

  const loadBook = async () => {
    try {
      const data = await BooksService.getBook(id);
      setBook(data);
    } catch (error) {
      console.error("Error loading book:", error);
      Alert.alert("Error", "Failed to load book details");
      router.back();
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    Alert.alert("Delete Book", "Are you sure you want to delete this book?", [
      { text: "Cancel", style: "cancel" },
      {
        text: "Delete",
        style: "destructive",
        onPress: async () => {
          try {
            await BooksService.deleteBook(id);
            router.replace("/books");
          } catch (error) {
            console.error("Error deleting book:", error);
            Alert.alert("Error", "Failed to delete book");
          }
        },
      },
    ]);
  };

  if (loading) {
    return (
      <ThemedView style={styles.loadingContainer}>
        <ActivityIndicator size="large" />
      </ThemedView>
    );
  }

  if (!book) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Book not found</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ScrollView>
      <ThemedView style={styles.container}>
        <ThemedText type="title">{book.title}</ThemedText>
        <ThemedText style={styles.description}>{book.description}</ThemedText>

        <ThemedText type="subtitle" style={styles.sectionTitle}>
          Chapters ({book.chapters.length})
        </ThemedText>

        {book.chapters.map((chapter, index) => (
          <ThemedView key={index} style={styles.chapterCard}>
            <ThemedText type="defaultSemiBold">{chapter.title}</ThemedText>
            <ThemedText numberOfLines={3}>{chapter.content}</ThemedText>
          </ThemedView>
        ))}

        {false && (
          <ThemedView style={styles.actions}>
            <Button onPress={() => router.push(`/books/${id}/edit`)}>
              Edit Book
            </Button>
            <Button onPress={handleDelete} variant="danger">
              Delete Book
            </Button>
          </ThemedView>
        )}
      </ThemedView>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    gap: 16,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  description: {
    fontSize: 16,
    lineHeight: 24,
  },
  sectionTitle: {
    marginTop: 8,
  },
  chapterCard: {
    padding: 16,
    borderRadius: 8,
    backgroundColor: "#f5f5f5",
    gap: 8,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 16,
  },
  editButton: {
    flex: 1,
  },
});
