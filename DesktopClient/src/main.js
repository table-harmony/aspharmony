import { app, BrowserWindow } from "electron";
import { fileURLToPath } from "url";
import { dirname, join } from "path";
import path from "path";

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    icon: path.join(__dirname, "assets", "logo.ico"),
    webPreferences: {
      nodeIntegration: true,
      contextIsolation: false,
      webSecurity: false,
    },
  });

  win.loadFile(path.join(__dirname, "../dist/index.html"));

  win.webContents.on("will-navigate", (event, url) => {
    event.preventDefault();
    const urlObj = new URL(url);
    win.loadFile(path.join(__dirname, "../dist/index.html"), {
      hash: urlObj.pathname + urlObj.search + urlObj.hash,
    });
  });

  win.webContents.setWindowOpenHandler(({ url }) => {
    if (url.startsWith("http")) {
      require("electron").shell.openExternal(url);
      return { action: "deny" };
    }
    return { action: "allow" };
  });
}

app.whenReady().then(createWindow);

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});

app.on("activate", () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
