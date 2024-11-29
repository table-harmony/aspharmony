import { Stack } from "expo-router";
import { useUserStore } from "@/stores/userStore";

export default function AppLayout() {
  return (
    <Stack>
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
  );
}
