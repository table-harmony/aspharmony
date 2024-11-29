import { useState } from "react";
import { StyleSheet, TextInput, Pressable, Alert } from "react-native";
import { Link, useRouter } from "expo-router";

import { ThemedText } from "@/components/ThemedText";
import { ThemedView } from "@/components/ThemedView";
import { login } from "@/services/auth";
import { useUserStore } from "@/stores/userStore";

export default function LoginScreen() {
  const [email, setemail] = useState("");
  const [password, setPassword] = useState("");
  const router = useRouter();
  const setUser = useUserStore((state) => state.setUser);

  async function handleLogin() {
    try {
      const user = await login(email, password);
      setUser(user);
      router.replace("/");
    } catch (error) {
      console.error("Login failed:", error);
      Alert.alert("Error", "Login failed. Please check your credentials.");
    }
  }

  return (
    <ThemedView style={styles.container}>
      <ThemedText type="title" style={styles.title}>
        Login
      </ThemedText>

      <ThemedView style={styles.form}>
        <ThemedView style={styles.field}>
          <ThemedText>email</ThemedText>
          <TextInput
            style={styles.input}
            value={email}
            onChangeText={setemail}
            autoCapitalize="none"
          />
        </ThemedView>

        <ThemedView style={styles.field}>
          <ThemedText>Password</ThemedText>
          <TextInput
            style={styles.input}
            value={password}
            onChangeText={setPassword}
            secureTextEntry
          />
        </ThemedView>

        <Pressable style={styles.button} onPress={handleLogin}>
          <ThemedText style={styles.buttonText}>Login</ThemedText>
        </Pressable>

        <Link href="/register" asChild>
          <Pressable style={styles.linkButton}>
            <ThemedText type="link">Don't have an account? Register</ThemedText>
          </Pressable>
        </Link>
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 16,
    justifyContent: "center",
  },
  title: {
    textAlign: "center",
    marginBottom: 32,
  },
  form: {
    gap: 16,
  },
  field: {
    gap: 8,
  },
  input: {
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 8,
    padding: 12,
    fontSize: 16,
  },
  button: {
    backgroundColor: "#0a7ea4",
    padding: 16,
    borderRadius: 8,
    alignItems: "center",
  },
  buttonText: {
    color: "white",
    fontSize: 16,
    fontWeight: "600",
  },
  linkButton: {
    alignItems: "center",
  },
});
