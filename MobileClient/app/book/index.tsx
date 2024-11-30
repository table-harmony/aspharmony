import { useCallback, useState } from "react";
import { StyleSheet, FlatList, Pressable, Image } from "react-native";
import { Link, useRouter } from "expo-router";
import { useFocusEffect } from "@react-navigation/native";
import { Card, TextInput } from "react-native-paper";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { getBooks } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book } from "@/services/books";

export default function BooksScreen() {
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const router = useRouter();
  const user = useUserStore((state) => state.user);

  async function fetchBooks() {
    try {
      const fetchedBooks = await getBooks();
      setBooks(fetchedBooks);
    } catch (error) {
      console.error("Failed to fetch books:", error);
    } finally {
      setLoading(false);
    }
  }

  useFocusEffect(
    useCallback(() => {
      fetchBooks();
    }, [])
  );

  const filteredBooks = books.filter(
    (book) =>
      book.metadata.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.author.username.toLowerCase().includes(searchQuery.toLowerCase())
  );

  const renderBook = ({ item }: { item: Book }) => (
    <Link href={{ pathname: "/book/[id]", params: { id: item.id } }} asChild>
      <Pressable style={styles.bookCard}>
        <Image
          source={{ uri: item.metadata.image_url }}
          style={styles.bookImage}
          defaultSource={require("@/assets/images/placeholder-book.png")}
        />
        <ThemedView style={styles.bookInfo}>
          <ThemedText type="subtitle" numberOfLines={1}>
            {item.metadata.title}
          </ThemedText>
          <ThemedText type="defaultSemiBold">
            By {item.author.username}
          </ThemedText>
          <ThemedText numberOfLines={2} style={styles.description}>
            {item.metadata.description}
          </ThemedText>
        </ThemedView>
      </Pressable>
    </Link>
  );

  if (loading) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading books...</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.header}>
        <ThemedText type="title">Books</ThemedText>
        {user && (
          <Pressable
            style={styles.createButton}
            onPress={() => router.push("/book/create")}
          >
            <ThemedText style={styles.createButtonText}>Create Book</ThemedText>
          </Pressable>
        )}
      </ThemedView>

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

      <FlatList
        data={filteredBooks}
        renderItem={renderBook}
        keyExtractor={(item) => item.id.toString()}
        contentContainerStyle={styles.listContainer}
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
  createButton: {
    backgroundColor: "#0a7ea4",
    padding: 12,
    borderRadius: 8,
  },
  createButtonText: {
    color: "white",
  },
  listContainer: {
    gap: 16,
  },
  bookCard: {
    flexDirection: "row",
    padding: 12,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: "#ccc",
    gap: 12,
  },
  bookImage: {
    width: 80,
    height: 120,
    borderRadius: 4,
  },
  bookInfo: {
    flex: 1,
    gap: 4,
  },
  description: {
    color: "#666",
  },
  searchCard: {
    marginBottom: 16,
  },
});
