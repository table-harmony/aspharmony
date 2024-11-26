import { useEffect, useState } from "react";
import {
  FlatList,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
} from "react-native";
import { Link } from "expo-router";

import { ThemedView } from "@/components/ThemedView";
import { ThemedText } from "@/components/ThemedText";
import { Book, BooksService } from "@/services/books";
import { Button } from "@/components/ui/Button";

export default function BooksScreen() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");

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
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadBooks();
  };

  const filteredBooks = books.filter(
    (book) =>
      book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.description.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const renderBook = ({ item }: { item: Book }) => (
    <Link href={`/books/${item.id}`} asChild>
      <ThemedView style={styles.bookCard}>
        <ThemedText type="subtitle">{item.title}</ThemedText>
        <ThemedText numberOfLines={2} style={styles.description}>
          {item.description}
        </ThemedText>
        <ThemedText type="defaultSemiBold">
          {item.chapters.length} chapters
        </ThemedText>
      </ThemedView>
    </Link>
  );

  if (loading) {
    return (
      <ThemedView style={styles.loadingContainer}>
        <ActivityIndicator size="large" />
      </ThemedView>
    );
  }

  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.header}>
        <ThemedText type="title">Books</ThemedText>
        <Link href="/books/create" asChild>
          <Button onPress={() => {}}>Create New</Button>
        </Link>
      </ThemedView>

      <FlatList
        data={filteredBooks}
        renderItem={renderBook}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={styles.list}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
      />
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  header: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 16,
  },
  searchBar: {
    marginBottom: 16,
  },
  list: {
    gap: 16,
  },
  bookCard: {
    padding: 16,
    borderRadius: 8,
    backgroundColor: "#f5f5f5",
    gap: 8,
  },
  description: {
    fontSize: 14,
    color: "#666",
  },
});
