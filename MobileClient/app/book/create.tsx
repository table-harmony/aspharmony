import { useEffect, useState } from "react";
import { Alert, ScrollView, StyleSheet } from "react-native";
import { useRouter } from "expo-router";
import {
  TextInput,
  Button,
  Surface,
  Text,
  useTheme,
  Portal,
  Dialog,
  IconButton,
  Card,
} from "react-native-paper";
import { Picker } from "@react-native-picker/picker";

import { createBook, getServers } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Chapter, CreateBookDto, ServerType } from "@/services/books";

export default function CreateBookScreen() {
  const router = useRouter();
  const user = useUserStore((state) => state.user);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [chapters, setChapters] = useState<Chapter[]>([]);
  const [servers, setServers] = useState<ServerType[]>([]);
  const [selectedServer, setSelectedServer] = useState<number>(1); // Default server

  const theme = useTheme();

  const loadServers = async () => {
    try {
      const data = await getServers();
      setServers(data);
      if (data.length > 0) {
        setSelectedServer(data[0].id);
      }
    } catch (error) {
      console.error("Failed to load servers:", error);
      Alert.alert("Error", "Failed to load available servers");
    }
  };

  useEffect(() => {
    loadServers();
  }, []);

  if (!user) {
    router.replace("/login");
    return null;
  }

  function addChapter() {
    const newChapter: Chapter = {
      index: chapters.length,
      title: "",
      content: "",
    };
    setChapters([...chapters, newChapter]);
  }

  function updateChapter(index: number, field: keyof Chapter, value: string) {
    const updatedChapters = [...chapters];
    updatedChapters[index] = {
      ...updatedChapters[index],
      [field]: value,
    };
    setChapters(updatedChapters);
  }

  function removeChapter(index: number) {
    const updatedChapters = chapters
      .filter((_, i) => i !== index)
      .map((chapter, i) => ({ ...chapter, index: i }));
    setChapters(updatedChapters);
  }

  async function handleCreate() {
    if (!user) {
      router.replace("/login");
      return;
    }

    if (!title.trim() || !description.trim()) {
      Alert.alert("Error", "Please fill in all required fields");
      return;
    }

    try {
      const newBook: CreateBookDto = {
        server: Number(selectedServer),
        author_id: user.id,
        audio_books: [],
        metadata: {
          title,
          description,
          image_url: "https://via.placeholder.com/400x600?text=No+Cover",
          chapters,
        },
      };

      await createBook(newBook);
      router.replace("/book");
    } catch (error) {
      console.error("Failed to create book:", error);
      Alert.alert("Error", "Failed to create book");
    }
  }

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <Text variant="headlineMedium">Create New Book</Text>

        <Card style={styles.field}>
          <Card.Content>
            <Text variant="titleMedium">Title</Text>
            <TextInput
              mode="outlined"
              value={title}
              onChangeText={setTitle}
              placeholder="Book title"
            />
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Text variant="titleMedium">Description</Text>
            <TextInput
              mode="outlined"
              value={description}
              onChangeText={setDescription}
              multiline
              numberOfLines={4}
              placeholder="Book description"
            />
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Text variant="titleMedium">Server</Text>
            <Surface style={styles.pickerContainer}>
              <Picker
                selectedValue={selectedServer}
                onValueChange={(value) => setSelectedServer(value)}
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

        <Card style={styles.chaptersSection}>
          <Card.Content>
            <Surface style={styles.chapterHeader} elevation={0}>
              <Text variant="titleMedium">Chapters</Text>
              <Button mode="contained" onPress={addChapter} icon="plus">
                Add Chapter
              </Button>
            </Surface>

            {chapters.map((chapter, index) => (
              <Surface key={chapter.index} style={styles.chapter} elevation={1}>
                <Surface style={styles.chapterTitleRow} elevation={0}>
                  <Text variant="titleMedium">Chapter {index + 1}</Text>
                  <IconButton
                    icon="delete"
                    mode="contained-tonal"
                    onPress={() => removeChapter(index)}
                    iconColor={theme.colors.error}
                  />
                </Surface>

                <TextInput
                  mode="outlined"
                  value={chapter.title}
                  onChangeText={(text) => updateChapter(index, "title", text)}
                  placeholder="Chapter title"
                />

                <TextInput
                  mode="outlined"
                  value={chapter.content}
                  onChangeText={(text) => updateChapter(index, "content", text)}
                  multiline
                  numberOfLines={4}
                  placeholder="Chapter content"
                />
              </Surface>
            ))}
          </Card.Content>
        </Card>

        <Button
          mode="contained"
          onPress={handleCreate}
          style={styles.createButton}
        >
          Create Book
        </Button>
      </Surface>
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
    marginVertical: 8,
  },
  pickerContainer: {
    borderRadius: 8,
    overflow: "hidden",
    marginTop: 8,
  },
  picker: {
    backgroundColor: "#f5f5f5",
    height: 50,
  },
  chaptersSection: {
    marginVertical: 16,
  },
  chapterHeader: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    marginBottom: 16,
  },
  chapter: {
    padding: 16,
    borderRadius: 8,
    marginBottom: 16,
    gap: 12,
  },
  chapterTitleRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  createButton: {
    marginTop: 16,
  },
});
