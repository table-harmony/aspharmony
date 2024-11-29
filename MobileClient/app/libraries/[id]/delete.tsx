import { useState, useEffect } from "react";
import { StyleSheet, Alert } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import {
  Surface,
  Text,
  Button,
  Card,
  useTheme,
  Avatar,
} from "react-native-paper";

import { deleteLibrary, getLibrary } from "@/services/libraries";
import type { Library } from "@/services/libraries";

export default function DeleteLibraryScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [library, setLibrary] = useState<Library | null>(null);
  const theme = useTheme();

  useEffect(() => {
    async function fetchLibrary() {
      try {
        const fetchedLibrary = await getLibrary(Number(id));
        setLibrary(fetchedLibrary);
      } catch (error) {
        console.error("Failed to fetch library:", error);
        Alert.alert("Error", "Failed to load library details");
        router.back();
      }
    }
    fetchLibrary();
  }, [id]);

  async function handleDelete() {
    try {
      await deleteLibrary(Number(id));
      router.replace("/libraries");
    } catch (error) {
      console.error("Failed to delete library:", error);
      Alert.alert("Error", "Failed to delete library");
    }
  }

  if (!library) {
    return null;
  }

  return (
    <Surface style={styles.container}>
      <Card style={styles.card}>
        <Card.Content style={styles.content}>
          <Surface style={styles.warningContainer} elevation={0}>
            <Avatar.Icon
              icon="alert"
              size={64}
              style={styles.warningIcon}
              color={theme.colors.error}
            />
            <Text variant="headlineSmall" style={styles.title}>
              Delete Library
            </Text>
            <Text variant="titleLarge" style={styles.libraryTitle}>
              "{library.name}"
            </Text>
            <Text variant="bodyLarge" style={styles.warningText}>
              Are you sure you want to delete this library? This action cannot
              be undone.
            </Text>
            <Text variant="bodyMedium" style={styles.metadata}>
              {library.books.length} Books • {library.members.length} Members
            </Text>
          </Surface>

          <Surface style={styles.actions} elevation={0}>
            <Button
              mode="contained-tonal"
              icon="close"
              onPress={() => router.back()}
              style={styles.button}
              buttonColor={theme.colors.primary}
            >
              Cancel
            </Button>
            <Button
              mode="contained"
              icon="delete"
              onPress={handleDelete}
              buttonColor={theme.colors.error}
              style={styles.button}
            >
              Delete
            </Button>
          </Surface>
        </Card.Content>
      </Card>
    </Surface>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    justifyContent: "center",
    paddingBottom: 32,
  },
  card: {
    overflow: "hidden",
  },
  content: {
    padding: 24,
    alignItems: "center",
    gap: 24,
  },
  warningContainer: {
    alignItems: "center",
    gap: 16,
    padding: 16,
  },
  warningIcon: {
    backgroundColor: "transparent",
  },
  warningText: {
    textAlign: "center",
    opacity: 0.7,
  },
  title: {
    fontWeight: "bold",
  },
  libraryTitle: {
    textAlign: "center",
    fontStyle: "italic",
  },
  metadata: {
    textAlign: "center",
    opacity: 0.7,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 8,
    width: "100%",
  },
  button: {
    flex: 1,
  },
});
