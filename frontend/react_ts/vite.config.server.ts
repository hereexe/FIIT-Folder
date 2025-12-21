import { defineConfig } from "vite";
import path from "path";

// Server build configuration
export default defineConfig({
  build: {
    lib: {
      entry: path.resolve(__dirname, "server/node-build.ts"),
      name: "server",
      fileName: "production",
      formats: ["es"],
    },
    outDir: "dist/server",
    target: "node22",
    ssr: true,
    rollupOptions: {
      external: [
        // Node.js built-ins
        "fs",
        "path",
        "url",
        "http",
        "https",
        "os",
        "crypto",
        "stream",
        "util",
        "events",
        "buffer",
        "querystring",
        "child_process",
        // External dependencies that should not be bundled
        "express",
        "cors",
      ],
      output: {
        format: "es",
        entryFileNames: "[name].mjs",
      },
    },
    minify: false, // Keep readable for debugging
    sourcemap: true,
  },
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
  define: {
    "process.env.NODE_ENV": '"production"',
  },
});
