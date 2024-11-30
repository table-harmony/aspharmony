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
import {
  Button,
  Card,
  Surface,
  Avatar,
  useTheme,
  IconButton,
} from "react-native-paper";

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
  const theme = useTheme();

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

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.fieldHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="book"
                style={styles.fieldIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="subtitle">Title</ThemedText>
            </Surface>
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
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.fieldHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="text"
                style={styles.fieldIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="subtitle">Description</ThemedText>
            </Surface>
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
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.fieldHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="server"
                style={styles.fieldIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="subtitle">Server</ThemedText>
            </Surface>
            <Surface style={styles.pickerContainer}>
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
            </Surface>
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.sectionHeader} elevation={0}>
              <Surface style={styles.chapterHeaderLeft} elevation={0}>
                <Avatar.Icon
                  size={24}
                  icon="book-open-variant"
                  style={styles.fieldIcon}
                  color={theme.colors.primary}
                />
                <ThemedText type="subtitle">Chapters</ThemedText>
              </Surface>
            </Surface>

            {book.metadata.chapters.map((chapter, index) => (
              <ThemedView key={chapter.index} style={styles.chapter}>
                <Surface style={styles.chapterHeader} elevation={0}>
                  <Surface style={styles.chapterHeaderLeft} elevation={0}>
                    <Avatar.Icon
                      size={24}
                      icon="bookmark"
                      style={styles.fieldIcon}
                      color={theme.colors.primary}
                    />
                    <ThemedText type="subtitle">Chapter {index + 1}</ThemedText>
                  </Surface>
                </Surface>

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
          </Card.Content>
        </Card>

        <Button
          mode="contained"
          onPress={handleSave}
          icon="content-save"
          style={styles.saveButton}
        >
          Save Changes
        </Button>
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
    marginBottom: 8,
  },
  chapter: {
    gap: 8,
    padding: 12,
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    marginBottom: 8,
  },
  chapterTitleRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  addButton: {
    padding: 8,
    borderRadius: 8,
  },
  removeButton: {
    padding: 8,
    borderRadius: 8,
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
  fieldHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginBottom: 12,
  },
  fieldIcon: {
    backgroundColor: "transparent",
  },
  chapterHeaderLeft: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
  },
  sectionHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 16,
  },
  addChapterButton: {
    minWidth: 130,
  },
  saveButton: {
    alignSelf: "center",
    marginTop: 16,
    borderRadius: 8,
    alignItems: "center",
  },
});
