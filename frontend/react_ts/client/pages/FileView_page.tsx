import FileViewer from "@/components/FileView";

export default function Index() {
    return (
      <div className="min-h-screen bg-folder-gradient">
        <main className="py-6">
          <FileViewer
            title="2022. Расписанные билеты"
            author="Artem Scheglevatov"
            description="Расписанные билеты для коллоквиума по матану за 2023 год, лектор И. Е. Симонов"
            likes={202}
            dislikes={11}
          />
        </main>
      </div>
    );
  }