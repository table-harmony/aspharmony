import { NavigationContainer } from "@react-navigation/native";
import { createNativeStackNavigator } from "@react-navigation/native-stack";
import { LoginPage } from "./pages/Login";
import { RegisterPage } from "./pages/Register";
import { HomePage } from "./pages/Home";
import { AuthGuard } from "./components/AuthGuard";

const Stack = createNativeStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen name="Login">
          {() => (
            <AuthGuard requireAuth={false}>
              <LoginPage />
            </AuthGuard>
          )}
        </Stack.Screen>
        <Stack.Screen name="Register">
          {() => (
            <AuthGuard requireAuth={false}>
              <RegisterPage />
            </AuthGuard>
          )}
        </Stack.Screen>
        <Stack.Screen name="Home">
          {() => (
            <AuthGuard>
              <HomePage />
            </AuthGuard>
          )}
        </Stack.Screen>
      </Stack.Navigator>
    </NavigationContainer>
  );
}
