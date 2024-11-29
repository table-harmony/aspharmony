import { Stack } from "expo-router";
import {
  PaperProvider,
  MD3LightTheme,
  configureFonts,
} from "react-native-paper";
import { useMemo } from "react";
import { useColorScheme } from "@/hooks/useColorScheme";

export default function AppLayout() {
  const colorScheme = useColorScheme() ?? "light";

  const theme = useMemo(
    () => ({
      ...MD3LightTheme,
      colors: {
        ...MD3LightTheme.colors,
        primary: "#0a7ea4",
        secondary: "#198754",
        error: "#dc3545",
        icon: colorScheme === "light" ? "#687076" : "#9BA1A6",
        background: colorScheme === "light" ? "#fff" : "#151718",
        text: colorScheme === "light" ? "#11181C" : "#ECEDEE",
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
          name="book/edit/[id]"
          options={{
            title: "Edit Book",
          }}
        />
        <Stack.Screen
          name="book/delete/[id]"
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
      </Stack>
    </PaperProvider>
  );
}
