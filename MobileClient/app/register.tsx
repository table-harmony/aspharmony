import { useState } from "react";
import { StyleSheet, Alert } from "react-native";
import { useRouter } from "expo-router";
import {
  TextInput,
  Button,
  Surface,
  Text,
  Card,
  useTheme,
  Avatar,
} from "react-native-paper";

import { register } from "@/services/auth";
import { useUserStore } from "@/stores/userStore";

export default function RegisterScreen() {
  const router = useRouter();
  const theme = useTheme();
  const setUser = useUserStore((state) => state.setUser);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  async function handleRegister() {
    if (!username.trim() || !password.trim() || !confirmPassword.trim()) {
      Alert.alert("Error", "Please fill in all fields");
      return;
    }

    if (password !== confirmPassword) {
      Alert.alert("Error", "Passwords do not match");
      return;
    }

    setLoading(true);
    try {
      const user = await register(username, password);
      setUser(user);
      router.replace("/book");
    } catch (error) {
      console.error("Registration failed:", error);
      Alert.alert("Error", "Registration failed. Please try again.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Surface style={styles.container}>
      <Card style={styles.card}>
        <Card.Content style={styles.content}>
          <Avatar.Icon
            icon="account-plus"
            size={64}
            style={styles.icon}
            color={theme.colors.primary}
          />
          <Text variant="headlineMedium" style={styles.title}>
            Create Account
          </Text>
          <Text variant="bodyLarge" style={styles.subtitle}>
            Sign up to get started
          </Text>

          <TextInput
            mode="outlined"
            label="Username"
            value={username}
            onChangeText={setUsername}
            autoCapitalize="none"
            left={<TextInput.Icon icon="account" />}
            style={styles.input}
          />

          <TextInput
            mode="outlined"
            label="Password"
            value={password}
            onChangeText={setPassword}
            secureTextEntry={!showPassword}
            right={
              <TextInput.Icon
                icon={showPassword ? "eye-off" : "eye"}
                onPress={() => setShowPassword(!showPassword)}
              />
            }
            left={<TextInput.Icon icon="lock" />}
            style={styles.input}
          />

          <TextInput
            mode="outlined"
            label="Confirm Password"
            value={confirmPassword}
            onChangeText={setConfirmPassword}
            secureTextEntry={!showPassword}
            left={<TextInput.Icon icon="lock-check" />}
            style={styles.input}
          />

          <Button
            mode="contained"
            onPress={handleRegister}
            loading={loading}
            style={styles.button}
          >
            Register
          </Button>

          <Surface style={styles.footer} elevation={0}>
            <Text variant="bodyMedium">Already have an account? </Text>
            <Button
              mode="text"
              onPress={() => router.push("/login")}
              style={styles.linkButton}
            >
              Login
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
  },
  card: {
    maxWidth: 400,
    width: "100%",
    alignSelf: "center",
  },
  content: {
    gap: 16,
    alignItems: "center",
    padding: 24,
  },
  icon: {
    backgroundColor: "transparent",
    marginBottom: 8,
  },
  title: {
    fontWeight: "bold",
  },
  subtitle: {
    opacity: 0.7,
    marginBottom: 8,
  },
  input: {
    width: "100%",
  },
  button: {
    width: "100%",
    marginTop: 8,
  },
  footer: {
    flexDirection: "row",
    alignItems: "center",
    marginTop: 8,
  },
  linkButton: {
    marginLeft: -8,
  },
});
