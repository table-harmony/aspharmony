import { useState, useEffect } from "react";
import { StyleSheet, ScrollView, Alert } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import {
  TextInput,
  Button,
  Switch,
  Surface,
  Card,
  Avatar,
  useTheme,
} from "react-native-paper";
import { Library, getLibrary, updateLibrary } from "@/services/libraries";
import { LoadingScreen } from "@/components/LoadingScreen";
import { ErrorScreen } from "@/components/ErrorScreen";
import { ThemedText } from "@/components/ThemedText";

export default function EditLibraryScreen() {
  const [library, setLibrary] = useState<Library>();
  const [name, setName] = useState("");
  const [allowCopies, setAllowCopies] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();
  const router = useRouter();
  const theme = useTheme();
  const { id } = useLocalSearchParams<{ id: string }>();
  const libraryId = parseInt(id);

  useEffect(() => {
    loadLibrary();
  }, [libraryId]);

  async function loadLibrary() {
    try {
      setLoading(true);
      const data = await getLibrary(libraryId);
      setLibrary(data);
      setName(data.name);
      setAllowCopies(data.allow_copies);
    } catch (err) {
      setError("Failed to load library");
    } finally {
      setLoading(false);
    }
  }

  async function handleSubmit() {
    try {
      await updateLibrary({
        id: libraryId,
        name,
        allow_copies: allowCopies,
      });
      router.back();
    } catch (err) {
      console.error("Failed to update library:", err);
      Alert.alert("Error", "Failed to update library");
    }
  }

  if (loading) return <LoadingScreen />;
  if (error) return <ErrorScreen message={error} onRetry={loadLibrary} />;
  if (!library) return null;

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <ThemedText type="title">Edit Library</ThemedText>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.fieldHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="book"
                style={styles.fieldIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="defaultSemiBold">Library Name</ThemedText>
            </Surface>
            <TextInput
              mode="outlined"
              value={name}
              onChangeText={setName}
              placeholder="Enter library name"
            />
          </Card.Content>
        </Card>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.switchContainer} elevation={0}>
              <Surface style={styles.switchHeader} elevation={0}>
                <Avatar.Icon
                  size={24}
                  icon="book-multiple"
                  style={styles.fieldIcon}
                  color={theme.colors.primary}
                />
                <ThemedText type="defaultSemiBold">
                  Allow Multiple Copies
                </ThemedText>
              </Surface>
              <Switch value={allowCopies} onValueChange={setAllowCopies} />
            </Surface>
            <ThemedText style={styles.switchDescription}>
              If enabled, multiple copies of the same book can be added to the
              library
            </ThemedText>
          </Card.Content>
        </Card>

        <Button
          mode="contained"
          onPress={handleSubmit}
          style={styles.button}
          disabled={!name.trim()}
        >
          Save Changes
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
  fieldHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginBottom: 8,
  },
  fieldIcon: {
    backgroundColor: "transparent",
  },
  switchContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  switchHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
  },
  switchDescription: {
    marginTop: 8,
    opacity: 0.7,
  },
  button: {
    marginTop: 16,
  },
});
