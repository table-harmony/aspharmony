import { Stack } from "expo-router";
import {
  PaperProvider,
  MD3LightTheme,
  MD3DarkTheme,
  configureFonts,
} from "react-native-paper";
import { useMemo } from "react";
import { useColorScheme } from "@/hooks/useColorScheme";

export default function AppLayout() {
  const colorScheme = useColorScheme() ?? "light";

  const theme = useMemo(
    () => ({
      ...(colorScheme === "light" ? MD3LightTheme : MD3DarkTheme),
      colors: {
        ...(colorScheme === "light"
          ? MD3LightTheme.colors
          : MD3DarkTheme.colors),
        primary: "#0a7ea4",
        secondary: "#198754",
        error: "#dc3545",
        icon: colorScheme === "light" ? "#687076" : "#9BA1A6",
        background: colorScheme === "light" ? "#fff" : "#151718",
        text: colorScheme === "light" ? "#11181C" : "#ECEDEE",
        surface: colorScheme === "light" ? "#fff" : "#151718",
        surfaceVariant: colorScheme === "light" ? "#ffffff" : "#1e1f20",
        elevation: {
          level0: colorScheme === "light" ? "#fff" : "#151718",
          level1: colorScheme === "light" ? "#ffffff" : "#1e1f20",
          level2: colorScheme === "light" ? "#ffffff" : "#252628",
          level3: colorScheme === "light" ? "#f8f9fa" : "#2a2b2d",
          level4: colorScheme === "light" ? "#f3f4f6" : "#2f3032",
          level5: colorScheme === "light" ? "#e9ecef" : "#343537",
        },
        onSurface: colorScheme === "light" ? "#11181C" : "#ECEDEE",
        onSurfaceVariant: colorScheme === "light" ? "#687076" : "#9BA1A6",
        outline: colorScheme === "light" ? "#ccc" : "#2f3032",
      },
      fonts: configureFonts({
        config: {
          regular: {
            fontFamily: "System",
            fontWeight: "400",
            fontSize: 16,
            letterSpacing: 0,
            lineHeight: 24,
          },
          medium: {
            fontFamily: "System",
            fontWeight: "500",
            fontSize: 16,
            letterSpacing: 0,
            lineHeight: 24,
          },
          bold: {
            fontFamily: "System",
            fontWeight: "700",
            fontSize: 16,
            letterSpacing: 0,
            lineHeight: 24,
          },
        },
      }),
    }),
    [colorScheme]
  );

  return (
    <PaperProvider theme={theme}>
      <Stack
        screenOptions={{
          headerStyle: {
            backgroundColor: theme.colors.primary,
          },
          headerTintColor: "#fff",
          contentStyle: {
            backgroundColor: theme.colors.background,
          },
          headerShown: true,
        }}
      >
        <Stack.Screen
          name="index"
          options={{
            title: "Home",
          }}
        />
        <Stack.Screen
          name="book/index"
          options={{
            title: "Books",
          }}
        />
        <Stack.Screen
          name="book/[id]"
          options={{
            title: "Book Details",
          }}
        />
        <Stack.Screen
          name="book/create"
          options={{
            title: "Create Book",
          }}
        />
        <Stack.Screen
          name="book/[id]/edit"
          options={{
            title: "Edit Book",
          }}
        />
        <Stack.Screen
          name="book/[id]/delete"
          options={{
            title: "Delete Book",
            presentation: "modal",
          }}
        />
        <Stack.Screen
          name="login"
          options={{
            title: "Login",
          }}
        />
        <Stack.Screen
          name="register"
          options={{
            title: "Register",
          }}
        />
        <Stack.Screen
          name="libraries"
          options={{
            title: "Libraries",
          }}
        />
        <Stack.Screen
          name="libraries/create"
          options={{
            title: "Create Library",
          }}
        />
        <Stack.Screen
          name="libraries/[id]"
          options={{
            title: "Library Details",
          }}
        />
        <Stack.Screen
          name="libraries/[id]/edit"
          options={{
            title: "Edit Library",
          }}
        />
        <Stack.Screen
          name="libraries/[id]/manage"
          options={{
            title: "Manage Library",
          }}
        />
        <Stack.Screen
          name="libraries/[id]/delete"
          options={{
            title: "Delete Library",
          }}
        />
      </Stack>
    </PaperProvider>
  );
}
