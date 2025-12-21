import { Link, useNavigate } from 'react-router-dom';
import { useGetSubjectsQuery } from '../../api/api';

export default function Index() {
  const { data: subjectsData, isLoading, error } = useGetSubjectsQuery();
  const navigate = useNavigate();

  const handleClick = (subjectId: string, subjectName: string) => {
    sessionStorage.setItem('selectedSubjectId', subjectId);
    sessionStorage.setItem('selectedSubject', subjectName);
    navigate("/exam_type", { state: { subjectId: subjectId, subjectName: subjectName } });
  };

  if (isLoading) return <div className="min-h-screen bg-app-background flex items-center justify-center text-app-text">Загрузка предметов...</div>;
  if (error) return <div className="min-h-screen bg-app-background flex items-center justify-center text-app-text">Ошибка загрузки предметов</div>;

  const subjects = subjectsData || [];

  return (
    <div className="min-h-screen bg-app-background flex flex-col">
      {/* Main Content */}
      <main className="flex-1 w-full px-4 md:px-5 lg:px-6 py-8 md:py-[45px]">
        <div className="max-w-[1280px] mx-auto">
          {/* Grid of Subject Cards */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 md:gap-x-[66px] md:gap-y-[50px]">
            {subjects.map((subject, index) => (
              <Link
                to={"/exam_type"}
                onClick={() => handleClick(subject.id, subject.name)}
                key={subject.id}
                className="h-[105px] bg-app-purple rounded-[10px] flex items-center justify-center text-app-text text-lg md:text-[25px] font-medium tracking-[0.25px] leading-normal text-center hover:opacity-30 hover:scale-110 transition-all"
              >
                {subject.name.replace(' ', '\n')}
              </Link>
            ))}
          </div>
        </div>
      </main>
    </div>
  );
}
