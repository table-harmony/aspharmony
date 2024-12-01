import { SiteHeader } from "./site-header";
import { SiteFooter } from "./site-footer";
import { ThemeProvider } from "next-themes";
import { FadeIn } from "./fade-in";

export function SiteLayout({ children }: { children: React.ReactNode }) {
  return (
    <ThemeProvider
      attribute="class"
      defaultTheme="system"
      enableSystem
      disableTransitionOnChange
    >
      <div className="relative flex min-h-screen flex-col bg-background">
        <SiteHeader />
        <div className="flex-1">
          <FadeIn>{children}</FadeIn>
        </div>
        <SiteFooter />
      </div>
    </ThemeProvider>
  );
}
