import { useState } from "react";
import { StyleSheet, ScrollView, TextInput } from "react-native";
import { router } from "expo-router";

import { ThemedView } from "@/components/ThemedView";
import { ThemedText } from "@/components/ThemedText";
import { Button } from "@/components/ui/Button";
import { BooksService } from "@/services/books";

export default function CreateBookScreen() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [loading, setLoading] = useState(false);

  const handleCreate = async () => {
    if (!title || !description) {
      // Add error handling/validation
      return;
    }

    setLoading(true);
    try {
      await BooksService.createBook({
        title,
        description,
        chapters: [],
      });
      router.replace("/books");
    } catch (error) {
      console.error("Error creating book:", error);
      // Add error handling UI
    } finally {
      setLoading(false);
    }
  };

  return (
    <ScrollView>
      <ThemedView style={styles.container}>
        <ThemedText type="title">Create New Book</ThemedText>

        <ThemedView style={styles.formGroup}>
          <ThemedText type="subtitle">Title</ThemedText>
          <TextInput
            style={styles.input}
            value={title}
            onChangeText={setTitle}
            placeholder="Enter book title"
            placeholderTextColor="#666"
          />
        </ThemedView>

        <ThemedView style={styles.formGroup}>
          <ThemedText type="subtitle">Description</ThemedText>
          <TextInput
            style={[styles.input, styles.textArea]}
            value={description}
            onChangeText={setDescription}
            placeholder="Enter book description"
            placeholderTextColor="#666"
            multiline
            numberOfLines={4}
          />
        </ThemedView>

        <Button
          onPress={handleCreate}
          disabled={loading || !title || !description}
          loading={loading}
        >
          Create Book
        </Button>
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
  formGroup: {
    gap: 8,
  },
  input: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
    backgroundColor: "#fff",
  },
  textArea: {
    height: 100,
    textAlignVertical: "top",
  },
});
