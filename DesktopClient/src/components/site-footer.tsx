import { Link } from "react-router-dom";
import { ModeToggle } from "./mode-toggle";

export function SiteFooter() {
  return (
    <footer className="border-t">
      <div className="container max-w-6xl mx-auto px-4 sm:px-6 py-2 sm:py-4">
        <div className="flex h-14 items-center justify-between gap-4">
          <p className="text-balance text-sm font-semibold leading-loose text-muted-foreground">
            Built by{" "}
            <Link
              to="https://github.com/table-harmony"
              target="_blank"
              rel="noreferrer"
              className="font-bold underline underline-offset-4"
            >
              TableHarmony
            </Link>
            . The source code is available on{" "}
            <Link
              rel="noreferrer"
              target="_blank"
              className="font-bold underline underline-offset-4"
              to="https://github.com/table-harmony/aspharmony"
            >
              GitHub
            </Link>
            .
          </p>
          <ModeToggle />
        </div>
      </div>
    </footer>
  );
}
