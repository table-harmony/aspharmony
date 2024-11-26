import { useEffect, useState } from "react";
import { FlatList, StyleSheet } from "react-native";
import { Link } from "expo-router";

import { ThemedView } from "@/components/ThemedView";
import { ThemedText } from "@/components/ThemedText";
import { Book, BooksService } from "@/services/books";

export default function BooksScreen() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadBooks();
  }, []);

  const loadBooks = async () => {
    try {
      const data = await BooksService.getBooks();
      setBooks(data);
    } catch (error) {
      console.error("Error loading books:", error);
    } finally {
      setLoading(false);
    }
  };

  const renderBook = ({ item }: { item: Book }) => (
    <Link href={`/books/${item.id}`} asChild>
      <ThemedView style={styles.bookCard}>
        <ThemedText type="subtitle">{item.title}</ThemedText>
        <ThemedText numberOfLines={2}>{item.description}</ThemedText>
        <ThemedText type="defaultSemiBold">
          {item.chapters.length} chapters
        </ThemedText>
      </ThemedView>
    </Link>
  );

  if (loading) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading...</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.header}>
        <ThemedText type="title">Books</ThemedText>
        <Link href="/books/create" asChild>
          <ThemedText type="link">Create New</ThemedText>
        </Link>
      </ThemedView>
      <FlatList
        data={books}
        renderItem={renderBook}
        keyExtractor={(item) => item.id}
        contentContainerStyle={styles.list}
      />
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 16,
  },
  list: {
    gap: 16,
  },
  bookCard: {
    padding: 16,
    borderRadius: 8,
    gap: 8,
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 3.84,
    elevation: 5,
  },
});
