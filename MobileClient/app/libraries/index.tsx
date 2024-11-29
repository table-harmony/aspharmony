import { useCallback, useState } from "react";
import { View, FlatList, StyleSheet, ScrollView } from "react-native";
import { Library, getLibraries } from "@/services/libraries";
import {
  Button,
  Card,
  Text,
  useTheme,
  Surface,
  Avatar,
} from "react-native-paper";
import { LoadingScreen } from "@/components/LoadingScreen";
import { ErrorScreen } from "@/components/ErrorScreen";
import { useUserStore } from "@/stores/userStore";
import { useFocusEffect } from "@react-navigation/native";
import { router } from "expo-router";
import { ThemedText } from "@/components/ThemedText";

export default function LibrariesScreen() {
  const [libraries, setLibraries] = useState<Library[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();
  const theme = useTheme();
  const user = useUserStore((state) => state.user);

  const loadLibraries = useCallback(async () => {
    try {
      setLoading(true);
      setError(undefined);
      const data = await getLibraries();
      setLibraries(data);
    } catch (err) {
      console.error("Failed to load libraries:", err);
      setError("Failed to load libraries");
    } finally {
      setLoading(false);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      loadLibraries();
    }, [loadLibraries])
  );

  if (loading) return <LoadingScreen />;
  if (error) return <ErrorScreen message={error} onRetry={loadLibraries} />;

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
              <ThemedText type="title">Libraries</ThemedText>
            </Surface>
            <ThemedText type="subtitle" style={styles.metadata}>
              Browse our libraries and join or create your own
            </ThemedText>

            {user && (
              <View style={styles.actions}>
                <Button
                  mode="contained"
                  onPress={() => router.push("/libraries/create")}
                  style={styles.createButton}
                  icon="plus"
                >
                  Create
                </Button>
              </View>
            )}
          </Card.Content>
        </Card>

        <Card style={styles.section}>
          <Card.Content>
            {libraries.map((library) => (
              <Card
                key={library.id}
                style={styles.libraryCard}
                onPress={() => router.push(`/libraries/${library.id}`)}
              >
                <Card.Content>
                  <Surface style={styles.libraryHeader} elevation={0}>
                    <Avatar.Icon
                      size={36}
                      icon="library"
                      style={styles.libraryIcon}
                      color={theme.colors.primary}
                    />
                    <View style={styles.libraryInfo}>
                      <ThemedText type="defaultSemiBold">
                        {library.name}
                      </ThemedText>
                      <ThemedText type="defaultSemiBold" style={styles.copies}>
                        {library.allow_copies
                          ? "Allows multiple copies"
                          : "Single copy only"}
                      </ThemedText>
                    </View>
                  </Surface>
                </Card.Content>
              </Card>
            ))}
          </Card.Content>
        </Card>
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
    marginBottom: 16,
  },
  actions: {
    flexDirection: "row",
    gap: 16,
    marginTop: 8,
  },
  createButton: {
    marginTop: 8,
  },
  libraryCard: {
    marginVertical: 8,
  },
  libraryHeader: {
    flexDirection: "row",
    alignItems: "center",
    gap: 12,
  },
  libraryIcon: {
    backgroundColor: "transparent",
  },
  libraryInfo: {
    flex: 1,
    gap: 2,
  },
  libraryMetadata: {
    opacity: 0.7,
  },
  copies: {
    opacity: 0.5,
  },
});
