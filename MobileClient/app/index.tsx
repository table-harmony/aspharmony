import { StyleSheet, Pressable, Image } from "react-native";
import { useRouter } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { useUserStore } from "@/stores/userStore";

export default function HomeScreen() {
  const router = useRouter();
  const user = useUserStore((state) => state.user);

  return (
    <ThemedView style={styles.container}>
      <ThemedView style={styles.header}>
        <ThemedText type="title">Welcome to AspHarmony</ThemedText>
        {user && (
          <ThemedText type="subtitle">Hello, {user.userName}!</ThemedText>
        )}
      </ThemedView>

      <ThemedView style={styles.content}>
        <ThemedView style={styles.section}>
          <ThemedText type="subtitle">Quick Actions</ThemedText>
          <ThemedView style={styles.actions}>
            <Pressable
              style={styles.actionCard}
              onPress={() => router.push("/book")}
            >
              <Image
                source={require("@/assets/images/books-icon.png")}
                style={styles.actionIcon}
              />
              <ThemedText type="defaultSemiBold">Browse Books</ThemedText>
              <ThemedText style={styles.description}>
                Explore our collection of books
              </ThemedText>
            </Pressable>

            {user && (
              <Pressable
                style={styles.actionCard}
                onPress={() => router.push("/book/create")}
              >
                <Image
                  source={require("@/assets/images/create-icon.png")}
                  style={styles.actionIcon}
                />
                <ThemedText type="defaultSemiBold">Create Book</ThemedText>
                <ThemedText style={styles.description}>
                  Write and publish your own book
                </ThemedText>
              </Pressable>
            )}
          </ThemedView>
        </ThemedView>

        {!user && (
          <ThemedView style={styles.authSection}>
            <ThemedText type="subtitle">Get Started</ThemedText>
            <ThemedView style={styles.authButtons}>
              <Pressable
                style={styles.loginButton}
                onPress={() => router.push("/login")}
              >
                <ThemedText style={styles.buttonText}>Login</ThemedText>
              </Pressable>
              <Pressable
                style={styles.registerButton}
                onPress={() => router.push("/register")}
              >
                <ThemedText style={styles.buttonText}>Register</ThemedText>
              </Pressable>
            </ThemedView>
          </ThemedView>
        )}
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  header: {
    padding: 16,
    gap: 8,
    borderBottomWidth: 1,
    borderBottomColor: "#ccc",
  },
  content: {
    flex: 1,
    padding: 16,
    gap: 24,
  },
  section: {
    gap: 16,
  },
  actions: {
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 16,
  },
  actionCard: {
    flex: 1,
    minWidth: 150,
    padding: 16,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: "#ccc",
    alignItems: "center",
    gap: 8,
  },
  actionIcon: {
    width: 48,
    height: 48,
  },
  description: {
    textAlign: "center",
    color: "#666",
    fontSize: 14,
  },
  authSection: {
    gap: 16,
  },
  authButtons: {
    flexDirection: "row",
    gap: 16,
  },
  loginButton: {
    flex: 1,
    backgroundColor: "#0a7ea4",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  registerButton: {
    flex: 1,
    backgroundColor: "#198754",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontWeight: "600",
  },
});
