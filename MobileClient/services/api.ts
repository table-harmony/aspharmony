import axios from "axios";

import { Platform } from "react-native";

const localhost = Platform.select({
  web: "localhost",
  android: "192.168.1.132",
  ios: "192.168.1.132",
});

export const BASE_URL = __DEV__ ? `http://${localhost}:7137` : "PROD_URL";

export const api = axios.create({
  baseURL: `${BASE_URL}/api`,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});
