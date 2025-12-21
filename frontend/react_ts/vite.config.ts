/// <reference types="vitest" />
import { defineConfig, Plugin } from "vite";
import react from "@vitejs/plugin-react-swc";
import path from "path";
import { createServer } from "./server";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => ({
  server: {
    host: "::",
    port: 8080,
    fs: {
      allow: ["./", "./src"],
      deny: [".env", ".env.*", "*.{crt,pem}", "**/.git/**", "server/**"],
    },
  },
  build: {
    outDir: "dist/spa",
  },
  test: {
    globals: true,
    environment: "jsdom",
    setupFiles: "./src/setupTests.ts",
  },
  plugins: [react(), expressPlugin()],
  resolve: {
    alias: [
      { find: "@/components", replacement: path.resolve(__dirname, "./src/shared") },
      { find: "@/hooks", replacement: path.resolve(__dirname, "./src/shared/hooks") },
      { find: "@/lib", replacement: path.resolve(__dirname, "./src/shared/lib") },
      { find: "@app", replacement: path.resolve(__dirname, "./src/app") },
      { find: "@pages", replacement: path.resolve(__dirname, "./src/pages") },
      { find: "@features", replacement: path.resolve(__dirname, "./src/features") },
      { find: "@entities", replacement: path.resolve(__dirname, "./src/entities") },
      { find: "@widgets", replacement: path.resolve(__dirname, "./src/widgets") },
      { find: "@shared", replacement: path.resolve(__dirname, "./src/shared") },
      { find: "@", replacement: path.resolve(__dirname, "./src") },
    ],
  },
}));

function expressPlugin(): Plugin {
  return {
    name: "express-plugin",
    apply: "serve", // Only apply during development (serve mode)
    configureServer(server) {
      const app = createServer();

      // Add Express app as middleware to Vite dev server
      server.middlewares.use(app);
    },
  };
}
