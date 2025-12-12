import fs from 'fs';
import { JSDOM } from 'jsdom';
import path from 'path';
import { fileURLToPath } from 'url';

// отримуємо поточну директорію скрипта
const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

// шлях до SVG
const svgFile = path.join(__dirname, 'src', 'assets', 'icons', 'all-icons.svg');

// шлях куди зберігати JS
const jsOutputFile = path.join(__dirname, 'src', 'assets', 'icons', 'icons.js');

// читаємо SVG
const svgContent = fs.readFileSync(svgFile, 'utf-8');

// парсимо SVG
const dom = new JSDOM(svgContent);
const svgEl = dom.window.document.querySelector('svg');

const icons = {};

// проходимо по групах
svgEl.querySelectorAll('g[class]').forEach(group => {
  const className = group.getAttribute('class');
  if (!className || className === 'Vector') return;

  // замінюємо дефіси на camelCase
  let key = className.replace(/-([a-z])/g, (_, c) => c.toUpperCase());

  // якщо ключ JS зарезервований (import, default тощо), додаємо суфікс
  const reservedWords = [
    'import',
    'default',
    'export',
    'class',
    'function',
    'var',
    'let',
    'const',
  ];
  if (reservedWords.includes(key)) key += 'Icon';

  const paths = group.querySelectorAll('path');
  if (paths.length === 0) return;

  const pathsHtml = Array.from(paths)
    .map(p => p.outerHTML)
    .join('\n  ');
  icons[key] = `<g class="${className}">\n  ${pathsHtml}\n</g>`;
});

// генеруємо JS контент
let jsContent = 'export const icons = {\n';
for (const [key, value] of Object.entries(icons)) {
  jsContent += `  ${key}: \`${value}\`,\n`;
}
jsContent += '};\n';

// записуємо JS файл
fs.writeFileSync(jsOutputFile, jsContent, 'utf-8');
