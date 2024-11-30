import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { Separator } from "@/components/ui/separator";

export function BookDetailsSkeleton() {
  return (
    <div className="flex flex-col gap-12">
      <div className="flex flex-col gap-8 sm:flex-row sm:items-start">
        <Skeleton className="aspect-square h-[300px] w-full max-w-[300px] rounded-lg" />
        <div className="flex flex-1 flex-col gap-6">
          <div className="space-y-4">
            <Skeleton className="h-9 w-3/4" />
            <Skeleton className="h-6 w-1/2" />
            <div className="space-y-2">
              <Skeleton className="h-4 w-full" />
              <Skeleton className="h-4 w-full" />
              <Skeleton className="h-4 w-3/4" />
            </div>
            <div className="flex gap-4 pt-2">
              <Skeleton className="h-10 w-24" />
              <Skeleton className="h-10 w-24" />
            </div>
          </div>
        </div>
      </div>

      <Separator />

      <div className="space-y-6">
        <Skeleton className="h-8 w-32" />
        <div className="grid gap-4">
          {[1, 2, 3].map((_, index) => (
            <Card key={index} className="overflow-hidden">
              <CardContent className="p-6">
                <div className="flex items-start gap-4">
                  <Skeleton className="h-8 w-8 rounded-full" />
                  <div className="space-y-2 flex-1">
                    <Skeleton className="h-5 w-1/4" />
                    <Skeleton className="h-4 w-full" />
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      </div>

      <Separator />

      <div className="space-y-6">
        <Skeleton className="h-8 w-32" />
        <div className="grid gap-4">
          {[1, 2].map((_, index) => (
            <Card key={index}>
              <CardContent className="flex items-center justify-between p-6">
                <div className="flex items-center gap-4">
                  <Skeleton className="h-10 w-10 rounded-full" />
                  <Skeleton className="h-6 w-32" />
                </div>
                <Skeleton className="h-10 w-24" />
              </CardContent>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
}
