import { StyleSheet, Image, ScrollView } from "react-native";
import { useRouter } from "expo-router";
import {
  Button,
  Surface,
  Text,
  Card,
  useTheme,
  Avatar,
  Divider,
} from "react-native-paper";

import { useUserStore } from "@/stores/userStore";

export default function HomeScreen() {
  const router = useRouter();
  const user = useUserStore((state) => state.user);
  const theme = useTheme();

  return (
    <ScrollView style={styles.container}>
      <Surface style={styles.hero} elevation={0}>
        <Avatar.Icon
          size={80}
          icon="book-open-page-variant"
          style={styles.heroIcon}
          color={theme.colors.primary}
        />
        <Text variant="headlineLarge" style={styles.heroTitle}>
          Welcome to AspHarmony
        </Text>
        <Text variant="bodyLarge" style={styles.heroSubtitle}>
          Your personal library in the cloud
        </Text>

        {user ? (
          <Surface style={styles.actionButtons} elevation={0}>
            <Button
              mode="contained"
              icon="book-multiple"
              onPress={() => router.push("/book")}
              style={styles.actionButton}
            >
              Books
            </Button>
            <Button
              mode="contained-tonal"
              icon="library"
              onPress={() => router.push("/libraries")}
              style={styles.actionButton}
            >
              Libraries
            </Button>
          </Surface>
        ) : (
          <Surface style={styles.authButtons} elevation={0}>
            <Button
              mode="contained"
              icon="login"
              onPress={() => router.push("/login")}
              style={styles.authButton}
            >
              Login
            </Button>
            <Button
              mode="contained-tonal"
              icon="account-plus"
              onPress={() => router.push("/register")}
              style={styles.authButton}
            >
              Register
            </Button>
          </Surface>
        )}
      </Surface>

      <Divider style={styles.divider} />

      <Surface style={styles.featuresContainer} elevation={0}>
        <Surface style={styles.sectionHeader} elevation={0}>
          <Text variant="titleLarge" style={styles.sectionTitle}>
            Features
          </Text>
        </Surface>

        <Surface style={styles.features} elevation={0}>
          <Card style={styles.featureCard}>
            <Card.Content>
              <Avatar.Icon
                icon="book-open-variant"
                size={48}
                style={styles.featureIcon}
                color={theme.colors.primary}
              />
              <Text variant="titleMedium">Digital Library</Text>
              <Text variant="bodyMedium">
                Store and organize your books in one place
              </Text>
            </Card.Content>
          </Card>

          <Card style={styles.featureCard}>
            <Card.Content>
              <Avatar.Icon
                icon="headphones"
                size={48}
                style={styles.featureIcon}
                color={theme.colors.primary}
              />
              <Text variant="titleMedium">Audio Support</Text>
              <Text variant="bodyMedium">Listen to your books on the go</Text>
            </Card.Content>
          </Card>

          <Card style={styles.featureCard}>
            <Card.Content>
              <Avatar.Icon
                icon="pencil"
                size={48}
                style={styles.featureIcon}
                color={theme.colors.primary}
              />
              <Text variant="titleMedium">Create & Share</Text>
              <Text variant="bodyMedium">
                Write and publish your own stories
              </Text>
            </Card.Content>
          </Card>
        </Surface>
      </Surface>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  hero: {
    padding: 24,
    paddingTop: 32,
    alignItems: "center",
    gap: 16,
  },
  heroImage: {
    width: 120,
    height: 120,
    marginBottom: 16,
  },
  heroTitle: {
    textAlign: "center",
    fontWeight: "bold",
  },
  heroSubtitle: {
    textAlign: "center",
    opacity: 0.7,
  },
  divider: {
    marginVertical: 32,
  },
  featuresContainer: {
    padding: 24,
    paddingBottom: 32,
  },
  sectionTitle: {
    marginBottom: 16,
    fontWeight: "bold",
  },
  features: {
    flexDirection: "row",
    flexWrap: "wrap",
    gap: 24,
    justifyContent: "center",
  },
  featureCard: {
    width: "45%",
    minWidth: 150,
    maxWidth: 200,
  },
  featureIcon: {
    backgroundColor: "transparent",
    marginBottom: 8,
  },
  ctaSection: {
    padding: 24,
    paddingBottom: 32,
    alignItems: "center",
    gap: 24,
    marginTop: "auto",
  },
  welcomeText: {
    textAlign: "center",
    marginBottom: 8,
  },
  ctaText: {
    textAlign: "center",
    marginBottom: 8,
  },
  actionButtons: {
    flexDirection: "row",
    gap: 16,
    marginTop: 16,
    width: "100%",
    maxWidth: 400,
  },
  actionButton: {
    flex: 1,
  },
  authButtons: {
    flexDirection: "row",
    gap: 16,
    marginTop: 16,
    width: "100%",
    maxWidth: 400,
  },
  authButton: {
    flex: 1,
  },
  sectionHeader: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center",
    gap: 8,
    marginBottom: 16,
  },
  sectionIcon: {
    backgroundColor: "transparent",
  },
  heroIcon: {
    backgroundColor: "transparent",
    marginBottom: 16,
  },
  description: {
    opacity: 0.7,
    marginTop: 4,
  },
});
