import React from "react";
import { StyleSheet, TouchableOpacity, View } from "react-native";
import { Link } from "expo-router";
import { ThemedText } from "./ThemedText";
import { ThemedView } from "./ThemedView";
import { IconSymbol } from "./ui/IconSymbol";
import { useAuth } from "@/contexts/AuthContext";

export function Navbar() {
  const { isAuthenticated, logout } = useAuth();

  return (
    <ThemedView style={styles.navbar}>
      <Link href="/" asChild>
        <TouchableOpacity style={styles.logoContainer}>
          <ThemedText style={styles.logo}>
            <ThemedText style={styles.highlight}>Asp</ThemedText>Harmony
          </ThemedText>
        </TouchableOpacity>
      </Link>

      <ThemedView style={styles.navLinks}>
        {isAuthenticated ? (
          <>
            <Link href="/books" asChild>
              <TouchableOpacity style={styles.navItem}>
                <IconSymbol name="books.vertical" size={20} color="#007AFF" />
                <ThemedText style={styles.navText}>All Books</ThemedText>
              </TouchableOpacity>
            </Link>
            <Link href="/libraries" asChild>
              <TouchableOpacity style={styles.navItem}>
                <IconSymbol name="building.columns" size={20} color="#007AFF" />
                <ThemedText style={styles.navText}>Libraries</ThemedText>
              </TouchableOpacity>
            </Link>
            <TouchableOpacity style={styles.navItem} onPress={logout}>
              <IconSymbol
                name="rectangle.portrait.and.arrow.right"
                size={20}
                color="#007AFF"
              />
              <ThemedText style={styles.navText}>Logout</ThemedText>
            </TouchableOpacity>
          </>
        ) : (
          <>
            <Link href="/login" asChild>
              <TouchableOpacity style={styles.navItem}>
                <IconSymbol name="person" size={20} color="#007AFF" />
                <ThemedText style={styles.navText}>Login</ThemedText>
              </TouchableOpacity>
            </Link>
            <Link href="/register" asChild>
              <TouchableOpacity style={styles.navItem}>
                <IconSymbol
                  name="person.badge.plus"
                  size={20}
                  color="#007AFF"
                />
                <ThemedText style={styles.navText}>Register</ThemedText>
              </TouchableOpacity>
            </Link>
          </>
        )}
      </ThemedView>
    </ThemedView>
  );
}

const styles = StyleSheet.create({
  navbar: {
    flexDirection: "row",
    justifyContent: "space-between",
    alignItems: "center",
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: "rgba(0,0,0,0.1)",
  },
  logoContainer: {
    flexDirection: "row",
    alignItems: "center",
  },
  logo: {
    fontSize: 20,
    fontWeight: "bold",
  },
  highlight: {
    color: "#007AFF",
  },
  navLinks: {
    flexDirection: "row",
    alignItems: "center",
    gap: 16,
  },
  navItem: {
    flexDirection: "row",
    alignItems: "center",
    gap: 4,
    padding: 8,
  },
  navText: {
    fontSize: 16,
    color: "#007AFF",
  },
});
