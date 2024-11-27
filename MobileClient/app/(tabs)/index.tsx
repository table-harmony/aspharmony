import {
  StyleSheet,
  Platform,
  TouchableOpacity,
  ScrollView,
  Text,
} from "react-native";
import { Link } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { useAuth } from "@/contexts/AuthContext";
import { IconSymbol } from "@/components/ui/IconSymbol";
import { Navbar } from "@/components/Navbar";
import { useUserStore } from "@/services/userStore";

export default function HomeScreen() {
  const { isAuthenticated, isLoading } = useAuth();
  const { user } = useUserStore();

  if (isLoading) {
    return (
      <ThemedView style={styles.container}>
        <ThemedText>Loading...</ThemedText>
      </ThemedView>
    );
  }

  return (
    <ScrollView>
      <Navbar />
      <ThemedView style={styles.container}>
        <ThemedView style={styles.heroSection}>
          <ThemedText style={styles.title}>
            <ThemedText style={styles.highlight}>Asp</ThemedText>Harmony
          </ThemedText>
          <ThemedText style={styles.subtitle}>
            Experience a modern approach to digital library management.
          </ThemedText>

          {!isAuthenticated && (
            <ThemedView style={styles.authButtons}>
              <Link href="/register" asChild>
                <TouchableOpacity style={styles.primaryButton}>
                  <IconSymbol
                    name="person.badge.plus"
                    size={20}
                    color="white"
                  />
                  <ThemedText style={styles.buttonText}>Register</ThemedText>
                </TouchableOpacity>
              </Link>
              <Link href="/login" asChild>
                <TouchableOpacity style={styles.secondaryButton}>
                  <IconSymbol
                    name="arrow.right.square"
                    size={20}
                    color="#007AFF"
                  />
                  <ThemedText style={styles.secondaryButtonText}>
                    Login
                  </ThemedText>
                </TouchableOpacity>
              </Link>
            </ThemedView>
          )}
        </ThemedView>
        <Text>{JSON.stringify(user)}</Text>
        {/* Features Section */}
        <ThemedView style={styles.featuresSection}>
          <ThemedText style={styles.sectionTitle}>Core Features</ThemedText>
          <ThemedView style={styles.featuresList}>
            <Feature icon="books.vertical" title="Browse Books" />
            <Feature icon="person.2" title="Join Libraries" />
            <Feature icon="arrow.up.doc" title="Share Books" />
            <Feature icon="paintbrush" title="Multiple Themes" />
          </ThemedView>
        </ThemedView>
      </ThemedView>
    </ScrollView>
  );
}

function Feature({ icon, title }: { icon: IconSymbolName; title: string }) {
  return (
    <ThemedView style={styles.featureItem}>
      <IconSymbol name={icon} size={32} color="#007AFF" />
      <ThemedText style={styles.featureTitle}>{title}</ThemedText>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
  },
  heroSection: {
    alignItems: "center",
    marginBottom: 40,
  },
  title: {
    fontSize: 36,
    fontWeight: "bold",
    marginBottom: 10,
    textAlign: "center",
    paddingTop: 100,
  },
  highlight: {
    color: "#007AFF",
  },
  subtitle: {
    fontSize: 18,
    textAlign: "center",
    marginBottom: 30,
    opacity: 0.8,
  },
  authButtons: {
    flexDirection: "row",
    gap: 10,
  },
  primaryButton: {
    backgroundColor: "#007AFF",
    flexDirection: "row",
    alignItems: "center",
    padding: 12,
    borderRadius: 8,
    gap: 8,
  },
  secondaryButton: {
    backgroundColor: "transparent",
    flexDirection: "row",
    alignItems: "center",
    padding: 12,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: "#007AFF",
    gap: 8,
  },
  buttonText: {
    color: "white",
    fontSize: 16,
    fontWeight: "600",
  },
  secondaryButtonText: {
    color: "#007AFF",
    fontSize: 16,
    fontWeight: "600",
  },
  featuresSection: {
    marginBottom: 40,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: "bold",
    marginBottom: 20,
    textAlign: "center",
  },
  featuresList: {
    flexDirection: "row",
    flexWrap: "wrap",
    justifyContent: "space-between",
    gap: 20,
  },
  featureItem: {
    width: "45%",
    alignItems: "center",
    padding: 15,
    borderRadius: 10,
    backgroundColor: Platform.select({
      ios: "rgba(255, 255, 255, 0.8)",
      android: "rgba(255, 255, 255, 0.9)",
      default: "rgba(255, 255, 255, 0.95)",
    }),
    shadowColor: "#000",
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.1,
    shadowRadius: 3,
    elevation: 3,
  },
  featureTitle: {
    marginTop: 8,
    fontSize: 16,
    fontWeight: "600",
    textAlign: "center",
  },
  reactLogo: {
    height: 178,
    width: 290,
    bottom: 0,
    left: 0,
    position: "absolute",
    opacity: 0.5,
  },
});
