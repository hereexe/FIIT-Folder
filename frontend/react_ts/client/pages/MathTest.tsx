import ReactMarkdown from 'react-markdown';
import remarkMath from 'remark-math';
import remarkGfm from 'remark-gfm';
import rehypeKatex from 'rehype-katex';
import 'katex/dist/katex.min.css';

const testMarkdown = `
# Теорема Больцано-Вейерштрасса
## Формулировка:

Каждая ограниченная последовательность точек пространства $\\mathbb{R}^m$ имеет сходящуюся подпоследовательность.

## Д-во:

Пусть $\\{x^n\\}$ – ограниченная последовательность.

Рассмотрим куб $\\Delta = \\{x: a \\leq x_k \\leq b\\}$, содержащий все точки $\\{x^n\\}$. $\\text{diam}_{\\infty} \\Delta = |b - a|$.

Разделим каждое ребро куба $\\Delta$ пополам. Обозначим $\\Delta^1$ любой из этих кубиков.

Заметим, что $\\text{diam}\\Delta^1 = \\dfrac{1}{2}\\text{diam}\\Delta$. Выберем некоторый элемент $x^{n_1} \\in \\Delta^1$.

При этом $\\text{diam}\\Delta^2 = \\dfrac{1}{2}\\text{diam}\\Delta^1 = \\dfrac{1}{4}\\text{diam}\\Delta$.

Существует единственная точка $c$, принадлежащая всем кубам $\\Delta^j$. Любая $\\varepsilon$-окрестность точки $c$ содержит все кубики.

$\\square$
`;

export default function MathTest() {
    return (
        <div className="p-8 max-w-4xl mx-auto">
            <h1 className="text-2xl font-bold mb-4">Тест рендеринга LaTeX (реальный контент)</h1>
            <div className="bg-white p-6 rounded-lg shadow-lg prose prose-lg max-w-none">
                <ReactMarkdown
                    remarkPlugins={[remarkMath, remarkGfm]}
                    rehypePlugins={[rehypeKatex]}
                >
                    {testMarkdown}
                </ReactMarkdown>
            </div>
        </div>
    );
}
