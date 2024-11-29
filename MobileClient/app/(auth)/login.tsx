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

import { login } from "@/services/auth";
import { useUserStore } from "@/stores/userStore";

export default function LoginScreen() {
  const router = useRouter();
  const theme = useTheme();
  const setUser = useUserStore((state) => state.setUser);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  async function handleLogin() {
    if (!email.trim() || !password.trim()) {
      Alert.alert("Error", "Please fill in all fields");
      return;
    }

    setLoading(true);
    try {
      const user = await login(email, password);
      setUser(user);
      router.replace("/");
    } catch (error) {
      console.error("Login failed:", error);
      Alert.alert("Error", "Login failed. Please check your credentials.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Surface style={styles.container}>
      <Card style={styles.card}>
        <Card.Content style={styles.content}>
          <Avatar.Icon
            icon="account"
            size={64}
            style={styles.icon}
            color={theme.colors.primary}
          />
          <Text variant="headlineMedium" style={styles.title}>
            Welcome Back
          </Text>
          <Text variant="bodyLarge" style={styles.subtitle}>
            Sign in to continue
          </Text>

          <TextInput
            mode="outlined"
            label="Email"
            value={email}
            onChangeText={setEmail}
            autoCapitalize="none"
            left={<TextInput.Icon icon="email" tabIndex={-1} />}
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
            left={<TextInput.Icon icon="lock" tabIndex={-1} />}
            style={styles.input}
          />

          <Button
            mode="contained"
            onPress={handleLogin}
            loading={loading}
            style={styles.button}
          >
            Login
          </Button>

          <Surface style={styles.footer} elevation={0}>
            <Text variant="bodyMedium">Don't have an account? </Text>
            <Button
              mode="text"
              onPress={() => router.push("/register")}
              style={styles.linkButton}
            >
              Register
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
