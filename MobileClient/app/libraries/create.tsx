import { useState } from "react";
import { StyleSheet, ScrollView, Alert } from "react-native";
import { useRouter } from "expo-router";
import {
  TextInput,
  Button,
  Switch,
  Surface,
  Card,
  Avatar,
  useTheme,
} from "react-native-paper";
import { createLibrary, joinLibrary } from "@/services/libraries";
import { ThemedText } from "@/components/ThemedText";
import { useUserStore } from "@/stores/userStore";

export default function CreateLibraryScreen() {
  const [name, setName] = useState("");
  const [allowCopies, setAllowCopies] = useState(false);
  const router = useRouter();
  const theme = useTheme();
  const user = useUserStore((state) => state.user);

  async function handleCreate() {
    if (!user) {
      router.replace("/login");
      return;
    }

    try {
      const library = await createLibrary({
        name,
        allow_copies: allowCopies,
      });

      await joinLibrary(library.id, user.id, "Manager");
      router.replace("/libraries");
    } catch (error) {
      console.error("Failed to create library:", error);
      Alert.alert("Error", "Failed to create library");
    }
  }

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <ThemedText type="title">Create New Library</ThemedText>

        <Card style={styles.field}>
          <Card.Content>
            <Surface style={styles.fieldHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="book"
                style={styles.fieldIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="defaultSemiBold">Name</ThemedText>
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
          onPress={handleCreate}
          style={styles.createButton}
          disabled={!name.trim()}
        >
          Create
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
  createButton: {
    marginTop: 16,
  },
});
