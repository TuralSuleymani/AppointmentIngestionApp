/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_BASE_URL: string;
  // add more VITE_ variables here if needed
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
