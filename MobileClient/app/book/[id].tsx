import { useCallback, useEffect, useState } from "react";
import { StyleSheet, Alert, Pressable, Image, ScrollView } from "react-native";
import { useFocusEffect, useLocalSearchParams, useRouter } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { getBookById, deleteBook } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book } from "@/services/books";

export default function BookScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useUserStore((state) => state.user);

  async function fetchBook() {
    try {
      const fetchedBook = await getBookById(Number(id));
      setBook(fetchedBook);
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

  if (loading || !book) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading book details...</ThemedText>
      </ThemedView>
    );
  }

  const isOwner = book.author.id === user?.id;

  return (
    <ScrollView style={styles.container}>
      <ThemedView style={styles.content}>
        <Image
          source={{ uri: book.metadata.image_url }}
          style={styles.coverImage}
          defaultSource={require("@/assets/images/placeholder-book.png")}
        />
        {book.metadata.image_url.includes("placeholder") && (
          <ThemedText style={styles.placeholderText}>
            No cover image available
          </ThemedText>
        )}

        <ThemedText type="title">{book.metadata.title}</ThemedText>
        <ThemedText type="subtitle">By {book.author.username}</ThemedText>

        <ThemedView style={styles.section}>
          <ThemedText type="subtitle">Description</ThemedText>
          <ThemedText>{book.metadata.description}</ThemedText>
        </ThemedView>

        <ThemedView style={styles.section}>
          <ThemedText type="subtitle">Chapters</ThemedText>
          {book.metadata.chapters.length === 0 ? (
            <ThemedText style={styles.placeholderText}>
              No chapters available
            </ThemedText>
          ) : (
            book.metadata.chapters.map((chapter) => (
              <ThemedView key={chapter.index} style={styles.chapter}>
                <ThemedText type="defaultSemiBold">
                  Chapter {chapter.index + 1}: {chapter.title}
                </ThemedText>
                <ThemedText>{chapter.content}</ThemedText>
              </ThemedView>
            ))
          )}
        </ThemedView>

        {isOwner && (
          <ThemedView style={styles.actions}>
            <Pressable
              style={styles.editButton}
              onPress={() =>
                router.push({
                  pathname: "/book/edit/[id]",
                  params: { id: book.id },
                })
              }
            >
              <ThemedText style={styles.buttonText}>Edit Book</ThemedText>
            </Pressable>
            <Pressable
              style={styles.deleteButton}
              onPress={() =>
                router.push({
                  pathname: "/book/delete/[id]",
                  params: { id: book.id },
                })
              }
            >
              <ThemedText style={styles.buttonText}>Delete Book</ThemedText>
            </Pressable>
          </ThemedView>
        )}
      </ThemedView>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  content: {
    padding: 16,
    gap: 16,
  },
  coverImage: {
    width: "100%",
    height: 300,
    borderRadius: 8,
  },
  section: {
    gap: 8,
  },
  chapter: {
    marginLeft: 16,
    gap: 4,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
  },
  editButton: {
    flex: 1,
    backgroundColor: "#0a7ea4",
    padding: 12,
    borderRadius: 8,
    alignItems: "center",
  },
  deleteButton: {
    flex: 1,
    backgroundColor: "#dc3545",
    padding: 12,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontWeight: "600",
  },
  placeholderText: {
    color: "#666",
    fontStyle: "italic",
    textAlign: "center",
  },
});
