import { useCallback, useState } from "react";
import { ScrollView, StyleSheet, Alert, View } from "react-native";
import { useLocalSearchParams, useRouter } from "expo-router";
import {
  Button,
  Card,
  Surface,
  useTheme,
  Avatar,
  IconButton,
  Portal,
  Dialog,
  Text,
} from "react-native-paper";
import {
  Library,
  getLibrary,
  promoteMember,
  demoteMember,
  removeMember,
} from "@/services/libraries";
import { LoadingScreen } from "@/components/LoadingScreen";
import { ErrorScreen } from "@/components/ErrorScreen";
import { ThemedText } from "@/components/ThemedText";
import { useFocusEffect } from "@react-navigation/native";
import { useUserStore } from "@/stores/userStore";

export default function ManageLibraryScreen() {
  const [library, setLibrary] = useState<Library>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();
  const [selectedMember, setSelectedMember] = useState<string>();
  const [actionDialogVisible, setActionDialogVisible] = useState(false);
  const { id } = useLocalSearchParams<{ id: string }>();
  const router = useRouter();
  const theme = useTheme();
  const user = useUserStore((state) => state.user);

  const libraryId = parseInt(id);

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

  async function handleMemberAction(
    action: "promote" | "demote" | "remove",
    memberId: string
  ) {
    if (!libraryId) return;

    try {
      switch (action) {
        case "promote":
          await promoteMember(libraryId, memberId);
          break;
        case "demote":
          await demoteMember(libraryId, memberId);
          break;
        case "remove":
          setSelectedMember(memberId);
          setActionDialogVisible(true);
          return;
      }
      loadLibrary();
    } catch (err) {
      console.error(`Failed to ${action} member:`, err);
      Alert.alert("Error", `Failed to ${action} member`);
    }
  }

  if (loading) return <LoadingScreen />;
  if (error) return <ErrorScreen message={error} onRetry={loadLibrary} />;
  if (!library) return null;

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.content} elevation={0}>
        <Card style={styles.section}>
          <Card.Content>
            <Surface style={styles.sectionHeader} elevation={0}>
              <ThemedText type="title" style={styles.metadata}>
                Manage '{library.name}'
              </ThemedText>
            </Surface>
            <ThemedText type="defaultSemiBold" style={styles.copies}>
              {library.allow_copies
                ? "Allows multiple copies"
                : "Single copy only"}
            </ThemedText>
            <View style={styles.actions}>
              <Button
                mode="contained-tonal"
                onPress={() => router.push(`/libraries/${libraryId}/add-books`)}
                style={styles.button}
                icon="book-plus"
              >
                Add Books
              </Button>
              <Button
                mode="contained"
                onPress={() => router.push(`/libraries/${libraryId}/edit`)}
                style={styles.button}
                icon="pencil"
              >
                Edit
              </Button>
              <Button
                mode="outlined"
                onPress={() => router.push(`/libraries/${libraryId}/delete`)}
                style={styles.button}
                textColor={theme.colors.error}
                icon="delete"
              >
                Delete
              </Button>
            </View>
          </Card.Content>
        </Card>

        <Card style={styles.section}>
          <Card.Content>
            <Surface style={styles.sectionHeader} elevation={0}>
              <Avatar.Icon
                size={36}
                icon="account-group"
                style={styles.sectionIcon}
                color={theme.colors.primary}
              />
              <ThemedText type="subtitle">Members</ThemedText>
            </Surface>
            <ThemedText type="defaultSemiBold" style={styles.description}>
              {library.members.length} Members • Manage roles and permissions
            </ThemedText>

            {library.members.map((member) => (
              <Card key={member.user.id} style={styles.memberCard}>
                <Card.Content style={styles.memberContent}>
                  <View style={styles.memberInfo}>
                    <Avatar.Icon
                      size={36}
                      icon={
                        member.role === "Manager" ? "account-star" : "account"
                      }
                      style={styles.memberIcon}
                      color={theme.colors.primary}
                    />
                    <View>
                      <ThemedText type="defaultSemiBold">
                        {member.user.username}
                      </ThemedText>
                      <ThemedText
                        type="defaultSemiBold"
                        style={styles.roleText}
                      >
                        {member.role}
                      </ThemedText>
                    </View>
                  </View>

                  {user?.id !== member.user.id && (
                    <View style={styles.memberActions}>
                      {member.role === "Member" && (
                        <IconButton
                          icon="arrow-up-bold"
                          size={20}
                          onPress={() =>
                            handleMemberAction("promote", member.user.id)
                          }
                        />
                      )}
                      {member.role === "Manager" && (
                        <IconButton
                          icon="arrow-down-bold"
                          size={20}
                          onPress={() =>
                            handleMemberAction("demote", member.user.id)
                          }
                        />
                      )}
                      <IconButton
                        icon="account-remove"
                        size={20}
                        iconColor={theme.colors.error}
                        onPress={() =>
                          handleMemberAction("remove", member.user.id)
                        }
                      />
                    </View>
                  )}
                </Card.Content>
              </Card>
            ))}
          </Card.Content>
        </Card>
      </Surface>

      <Portal>
        <Dialog
          visible={actionDialogVisible}
          onDismiss={() => setActionDialogVisible(false)}
        >
          <Dialog.Title>Remove Member</Dialog.Title>
          <Dialog.Content>
            <Text>Are you sure you want to remove this member?</Text>
          </Dialog.Content>
          <Dialog.Actions>
            <Button onPress={() => setActionDialogVisible(false)}>
              Cancel
            </Button>
            <Button
              textColor={theme.colors.error}
              onPress={async () => {
                if (selectedMember) {
                  await removeMember(libraryId, selectedMember);
                  setActionDialogVisible(false);
                  loadLibrary();
                }
              }}
            >
              Remove
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
    marginBottom: 8,
  },
  sectionIcon: {
    backgroundColor: "transparent",
  },
  actions: {
    flexDirection: "row",
    gap: 8,
    marginTop: 16,
  },
  metadata: {
    marginTop: 4,
    opacity: 0.7,
  },
  copies: {
    marginTop: 2,
    opacity: 0.5,
  },
  description: {
    marginBottom: 16,
    opacity: 0.7,
  },
  buttonGroup: {
    gap: 8,
  },
  button: {
    marginVertical: 4,
  },
  memberCard: {
    marginVertical: 8,
  },
  memberContent: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
  },
  memberInfo: {
    flexDirection: "row",
    alignItems: "center",
    gap: 12,
  },
  memberIcon: {
    backgroundColor: "transparent",
  },
  roleText: {
    opacity: 0.7,
  },
  memberActions: {
    flexDirection: "row",
    gap: 4,
  },
});
