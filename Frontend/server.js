import express from "express";
import path from "path";
import { fileURLToPath } from "url";
import { createProxyMiddleware } from "http-proxy-middleware";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const app = express();

// === PROXY for /api ===
app.use(
  "/api",
  createProxyMiddleware({
    target: "https://api-dobrodiy.kn314-uz.keenetic.pro",
    changeOrigin: true,
    secure: false,
  })
);

// === Serve Angular dist ===
app.use(express.static(path.join(__dirname, "dist/petcare/browser")));

app.get("*", (req, res) => {
  res.sendFile(path.join(__dirname, "dist/petcare/browser/index.html"));
});

const port = process.env.PORT || 8080;
app.listen(port, () => {
  console.log(`Frontend server running on port ${port}`);
});
