import { useCallback, useEffect, useState } from "react";
import {
  StyleSheet,
  TextInput,
  Alert,
  Pressable,
  ScrollView,
  View,
} from "react-native";
import { useFocusEffect, useLocalSearchParams, useRouter } from "expo-router";
import { Picker } from "@react-native-picker/picker";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import {
  getBookById,
  updateBook,
  ServerType,
  getServers,
} from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book, Chapter } from "@/services/books";

export default function EditBookScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<Book | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useUserStore((state) => state.user);
  const [servers, setServers] = useState<ServerType[]>([]);
  const [selectedServer, setSelectedServer] = useState<number>(0);

  async function loadServers() {
    try {
      const data = await getServers();
      setServers(data);
    } catch (error) {
      console.error("Failed to load servers:", error);
      Alert.alert("Error", "Failed to load available servers");
    }
  }

  async function fetchBook() {
    try {
      const fetchedBook = await getBookById(Number(id));
      if (fetchedBook.author.id !== user?.id) {
        Alert.alert("Error", "You don't have permission to edit this book");
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

  function updateChapter(index: number, field: keyof Chapter, value: string) {
    if (!book) return;

    const updatedChapters = [...book.metadata.chapters];
    updatedChapters[index] = {
      ...updatedChapters[index],
      [field]: value,
    };

    setBook({
      ...book,
      metadata: {
        ...book.metadata,
        chapters: updatedChapters,
      },
    });
  }

  function addChapter() {
    if (!book) return;

    const newChapter: Chapter = {
      index: book.metadata.chapters.length,
      title: "",
      content: "",
    };

    setBook({
      ...book,
      metadata: {
        ...book.metadata,
        chapters: [...book.metadata.chapters, newChapter],
      },
    });
  }

  function removeChapter(index: number) {
    if (!book) return;

    const updatedChapters = book.metadata.chapters
      .filter((_, i) => i !== index)
      .map((chapter, i) => ({ ...chapter, index: i }));

    setBook({
      ...book,
      metadata: {
        ...book.metadata,
        chapters: updatedChapters,
      },
    });
  }

  async function handleSave() {
    if (!book) return;

    try {
      await updateBook(book);
      router.back();
    } catch (error) {
      console.error("Failed to update book:", error);
      Alert.alert("Error", "Failed to save changes");
    }
  }

  useEffect(() => {
    loadServers();
    fetchBook();
  }, [id]);

  useFocusEffect(
    useCallback(() => {
      fetchBook();
    }, [])
  );

  const handleServerChange = async (serverId: number) => {
    if (!book) return;

    try {
      setSelectedServer(serverId);
      const updatedBook: Book = {
        ...book,
        server: Number(serverId),
      };
      setBook(updatedBook);
      Alert.alert("Success", "Server updated successfully");
    } catch (error) {
      Alert.alert("Error", "Failed to update server");
      setSelectedServer(book.server);
    }
  };

  if (loading || !book) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading...</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <ThemedView style={styles.content}>
        <ThemedText type="title">Edit Book</ThemedText>

        <ThemedView style={styles.field}>
          <ThemedText>Server</ThemedText>
          <View style={styles.pickerContainer}>
            <Picker
              selectedValue={book?.server}
              onValueChange={handleServerChange}
              style={styles.picker}
            >
              {servers.map((server) => (
                <Picker.Item
                  key={server.id}
                  label={server.display_name}
                  value={server.id}
                />
              ))}
            </Picker>
          </View>
        </ThemedView>

        <ThemedView style={styles.field}>
          <ThemedText>Title</ThemedText>
          <TextInput
            style={styles.input}
            value={book.metadata.title}
            onChangeText={(text) =>
              setBook({
                ...book,
                metadata: { ...book.metadata, title: text },
              })
            }
          />
        </ThemedView>

        <ThemedView style={styles.field}>
          <ThemedText>Description</ThemedText>
          <TextInput
            style={[styles.input, styles.textArea]}
            value={book.metadata.description}
            onChangeText={(text) =>
              setBook({
                ...book,
                metadata: { ...book.metadata, description: text },
              })
            }
            multiline
            numberOfLines={4}
          />
        </ThemedView>

        <ThemedView style={styles.chaptersSection}>
          <ThemedView style={styles.chapterHeader}>
            <ThemedText type="subtitle">Chapters</ThemedText>
            <Pressable style={styles.addButton} onPress={addChapter}>
              <ThemedText style={styles.buttonText}>Add Chapter</ThemedText>
            </Pressable>
          </ThemedView>

          {book.metadata.chapters.map((chapter, index) => (
            <ThemedView key={chapter.index} style={styles.chapter}>
              <ThemedView style={styles.chapterTitleRow}>
                <ThemedText type="defaultSemiBold">
                  Chapter {index + 1}
                </ThemedText>
                <Pressable
                  style={styles.removeButton}
                  onPress={() => removeChapter(index)}
                >
                  <ThemedText style={styles.buttonText}>Remove</ThemedText>
                </Pressable>
              </ThemedView>

              <TextInput
                style={styles.input}
                value={chapter.title}
                onChangeText={(text) => updateChapter(index, "title", text)}
                placeholder="Chapter title"
              />

              <TextInput
                style={[styles.input, styles.textArea]}
                value={chapter.content}
                onChangeText={(text) => updateChapter(index, "content", text)}
                multiline
                numberOfLines={4}
                placeholder="Chapter content"
              />
            </ThemedView>
          ))}
        </ThemedView>

        <Pressable style={styles.saveButton} onPress={handleSave}>
          <ThemedText style={styles.buttonText}>Save Changes</ThemedText>
        </Pressable>
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
  field: {
    gap: 8,
  },
  input: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
  },
  textArea: {
    height: 100,
    textAlignVertical: "top",
  },
  chaptersSection: {
    gap: 16,
  },
  chapterHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  chapter: {
    gap: 8,
    padding: 12,
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
  },
  chapterTitleRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  addButton: {
    backgroundColor: "#198754",
    padding: 8,
    borderRadius: 8,
  },
  removeButton: {
    backgroundColor: "#dc3545",
    padding: 8,
    borderRadius: 8,
  },
  saveButton: {
    backgroundColor: "#0a7ea4",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontWeight: "600",
  },
  pickerContainer: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    overflow: "hidden",
  },
  picker: {
    backgroundColor: "#f5f5f5",
    height: 50,
  },
});
