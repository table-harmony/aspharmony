import { Stack } from "expo-router";

export default function BooksLayout() {
  return (
    <Stack>
      <Stack.Screen
        name="index"
        options={{
          title: "Books",
        }}
      />
      <Stack.Screen
        name="create"
        options={{
          title: "Create Book",
        }}
      />
      <Stack.Screen
        name="[id]"
        options={{
          title: "Book Details",
        }}
      />
    </Stack>
  );
}
