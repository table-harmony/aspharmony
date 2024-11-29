import { useState } from "react";
import {
  StyleSheet,
  TextInput,
  Alert,
  Pressable,
  ScrollView,
} from "react-native";
import { useRouter } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { createBook } from "@/services/books";
import { useUserStore } from "@/stores/userStore";
import type { Book, Chapter, CreateBookDto } from "@/services/books";
import type { User } from "@/services/auth";

export default function CreateBookScreen() {
  const router = useRouter();
  const user = useUserStore((state) => state.user) as User;
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [chapters, setChapters] = useState<Chapter[]>([]);

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
    if (!title.trim() || !description.trim()) {
      Alert.alert("Error", "Please fill in all required fields");
      return;
    }

    try {
      const newBook: CreateBookDto = {
        server: 1,
        author_id: user.id,
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
      <ThemedView style={styles.content}>
        <ThemedText type="title">Create New Book</ThemedText>

        <ThemedView style={styles.field}>
          <ThemedText>Title</ThemedText>
          <TextInput
            style={styles.input}
            value={title}
            onChangeText={setTitle}
            placeholder="Book title"
          />
        </ThemedView>

        <ThemedView style={styles.field}>
          <ThemedText>Description</ThemedText>
          <TextInput
            style={[styles.input, styles.textArea]}
            value={description}
            onChangeText={setDescription}
            multiline
            numberOfLines={4}
            placeholder="Book description"
          />
        </ThemedView>

        <ThemedView style={styles.chaptersSection}>
          <ThemedView style={styles.chapterHeader}>
            <ThemedText type="subtitle">Chapters</ThemedText>
            <Pressable style={styles.addButton} onPress={addChapter}>
              <ThemedText style={styles.buttonText}>Add Chapter</ThemedText>
            </Pressable>
          </ThemedView>

          {chapters.map((chapter, index) => (
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

        <Pressable style={styles.createButton} onPress={handleCreate}>
          <ThemedText style={styles.buttonText}>Create Book</ThemedText>
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
  createButton: {
    backgroundColor: "#0a7ea4",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontWeight: "600",
  },
});
