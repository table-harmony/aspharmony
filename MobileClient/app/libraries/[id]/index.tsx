import { useCallback, useState } from "react";
import { View, StyleSheet, ScrollView, Alert } from "react-native";
import { useLocalSearchParams } from "expo-router";
import {
  Library,
  getLibrary,
  removeMember,
  joinLibrary,
} from "../../../services/libraries";
import {
  Button,
  Card,
  Text,
  useTheme,
  Dialog,
  Portal,
  Surface,
  Avatar,
} from "react-native-paper";
import { LoadingScreen } from "../../../components/LoadingScreen";
import { ErrorScreen } from "../../../components/ErrorScreen";
import { useUserStore } from "../../../stores/userStore";
import { useFocusEffect } from "@react-navigation/native";
import { router } from "expo-router";
import { Image } from "react-native";
import { ThemedText } from "@/components/ThemedText";

export default function LibraryDetailsScreen() {
  const [library, setLibrary] = useState<Library>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();
  const [leaveDialogVisible, setLeaveDialogVisible] = useState(false);
  const theme = useTheme();
  const user = useUserStore((state) => state.user);
  const { id } = useLocalSearchParams<{ id: string }>();

  const libraryId = parseInt(id);
  const userMembership = library?.members.find((m) => m.user.id === user?.id);
  const isManager = userMembership?.role === "Manager";
  const isMember = !!userMembership;

  const loadLibrary = useCallback(async () => {
    if (!libraryId || isNaN(libraryId)) {
      setError("Invalid library ID");
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setError(undefined);
      const data = await getLibrary(libraryId);
      setLibrary(data);
    } catch (err) {
      console.error("Failed to load library:", err);
      setError("Failed to load library");
    } finally {
      setLoading(false);
    }
  }, [libraryId]);

  useFocusEffect(
    useCallback(() => {
      loadLibrary();
    }, [loadLibrary])
  );

  async function handleLeave() {
    if (!user?.id || !libraryId) return;

    try {
      await removeMember(libraryId, user.id);
      router.back();
    } catch (err) {
      console.error("Failed to leave library:", err);
      setError("Failed to leave library");
    }
  }

  async function handleJoin() {
    if (!user?.id || !libraryId) return;

    try {
      await joinLibrary(libraryId, user.id);
      loadLibrary();
    } catch (err) {
      console.error("Failed to join library:", err);
      Alert.alert("Error", "Failed to join library");
    }
  }

  if (loading) return <LoadingScreen />;
  if (error) return <ErrorScreen message={error} onRetry={loadLibrary} />;
  if (!library) return null;

  const uniqueBooks = library.books.reduce((acc, curr) => {
    if (!acc.find((book) => book.book.id === curr.book.id)) {
      acc.push(curr);
    }
    return acc;
  }, [] as typeof library.books);

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <Card style={styles.section}>
          <Card.Content>
            <Surface style={styles.sectionHeader} elevation={0}>
              <Avatar.Icon
                size={48}
                icon="library"
                style={styles.sectionIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="title">{library.name}</ThemedText>
            </Surface>
            <ThemedText style={styles.metadata}>
              {library.books.length} Books • {library.members.length} Members
            </ThemedText>
            <ThemedText style={styles.copies}>
              {library.allow_copies
                ? "Allows multiple copies"
                : "Single copy only"}
            </ThemedText>

            <View style={styles.actions}>
              {isManager && (
                <Button
                  mode="contained"
                  onPress={() => router.push(`/libraries/${libraryId}/manage`)}
                  icon="cog"
                >
                  Manage
                </Button>
              )}
              {isMember ? (
                <Button
                  mode="outlined"
                  textColor={theme.colors.error}
                  onPress={() => setLeaveDialogVisible(true)}
                  icon="exit-to-app"
                >
                  Leave
                </Button>
              ) : (
                <Button
                  mode="contained"
                  onPress={handleJoin}
                  icon="account-plus"
                >
                  Join Library
                </Button>
              )}
            </View>
          </Card.Content>
        </Card>

        <Card style={styles.section}>
          <Card.Content>
            <Surface style={styles.sectionHeader} elevation={0}>
              <Avatar.Icon
                size={24}
                icon="book-multiple"
                style={styles.sectionIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="subtitle">Books</ThemedText>
            </Surface>

            {uniqueBooks.map((book) => (
              <Card
                key={book.id}
                style={styles.bookCard}
                onPress={() => router.push(`/book/${book.book_id}`)}
              >
                <Card.Content style={styles.bookContent}>
                  <Image
                    source={{ uri: book.book.metadata.image_url }}
                    style={styles.bookImage}
                  />
                  <View style={styles.bookInfo}>
                    <ThemedText type="defaultSemiBold">
                      {book.book.metadata.title}
                    </ThemedText>
                    <ThemedText style={styles.author}>
                      by {book.book.author.username}
                    </ThemedText>
                    {book.book.metadata.description && (
                      <ThemedText style={styles.description} numberOfLines={2}>
                        {book.book.metadata.description}
                      </ThemedText>
                    )}
                  </View>
                </Card.Content>
              </Card>
            ))}
          </Card.Content>
        </Card>
      </Surface>

      <Portal>
        <Dialog
          visible={leaveDialogVisible}
          onDismiss={() => setLeaveDialogVisible(false)}
        >
          <Dialog.Title>Leave Library</Dialog.Title>
          <Dialog.Content>
            <Text>Are you sure you want to leave this library?</Text>
          </Dialog.Content>
          <Dialog.Actions>
            <Button onPress={() => setLeaveDialogVisible(false)}>Cancel</Button>
            <Button textColor={theme.colors.error} onPress={handleLeave}>
              Leave
            </Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>
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
  section: {
    marginVertical: 8,
  },
  sectionHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 8,
    marginBottom: 16,
  },
  sectionIcon: {
    backgroundColor: "transparent",
  },
  metadata: {
    marginTop: 4,
    opacity: 0.7,
  },
  copies: {
    marginTop: 2,
    opacity: 0.5,
  },
  actions: {
    flexDirection: "row",
    gap: 8,
    marginTop: 16,
  },
  bookCard: {
    marginVertical: 8,
  },
  bookContent: {
    flexDirection: "row",
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
  author: {
    opacity: 0.7,
  },
  description: {
    opacity: 0.5,
  },
});
