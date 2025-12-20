import React, { useState } from "react";
import { useRegisterMutation } from "../../api/api";
import { useNavigate } from "react-router-dom";
import { ChevronLeft, Lock, User, AlertCircle, CheckCircle2 } from "lucide-react";

export default function Register_page() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [errorMsg, setErrorMsg] = useState("");
    const [isSuccess, setIsSuccess] = useState(false);
    const navigate = useNavigate();

    const [register, { isLoading }] = useRegisterMutation();

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrorMsg("");

        if (!username || !password) {
            setErrorMsg("Please fill in all fields");
            return;
        }

        if (password !== confirmPassword) {
            setErrorMsg("Passwords do not match");
            return;
        }

        try {
            await register({ username, password }).unwrap();
            setIsSuccess(true);
            setTimeout(() => {
                navigate("/login");
            }, 2000);
        } catch (err: any) {
            console.error("Registration failed:", err);
            setErrorMsg(err.data?.message || "Registration failed. Try a different username.");
        }
    };

    if (isSuccess) {
        return (
            <div className="min-h-screen bg-app-bg flex items-center justify-center p-4 font-[Inter]">
                <div className="w-full max-w-[450px] bg-white rounded-[25px] shadow-2xl p-10 text-center space-y-6">
                    <div className="flex justify-center">
                        <CheckCircle2 className="w-20 h-20 text-emerald-500 animate-bounce" />
                    </div>
                    <h1 className="text-3xl font-bold text-primary-dark">Успешно!</h1>
                    <p className="text-primary-dark/60 text-lg">
                        Вы успешно зарегистрированы. Сейчас вы будете перенаправлены на страницу входа.
                    </p>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-app-bg flex items-center justify-center p-4 font-[Inter]">
            <div className="w-full max-w-[450px] bg-white rounded-[25px] shadow-2xl overflow-hidden">
                {/* Header section with back button */}
                <div className="px-8 pt-8 pb-4">
                    <button
                        onClick={() => navigate("/login")}
                        className="flex items-center gap-2 text-primary-dark hover:opacity-70 transition-opacity mb-8"
                    >
                        <ChevronLeft className="w-6 h-6" />
                        <span className="text-lg font-medium">Назад</span>
                    </button>

                    <h1 className="text-4xl font-bold text-primary-dark tracking-tight mb-2">Регистрация</h1>
                    <p className="text-primary-dark/60 text-lg">Создайте аккаунт в FIIT Folder</p>
                </div>

                {/* Register Form */}
                <form onSubmit={handleRegister} className="px-8 pb-10 space-y-6">
                    {errorMsg && (
                        <div className="flex items-center gap-2 p-4 bg-red-50 text-red-600 rounded-[12px] border border-red-100">
                            <AlertCircle className="w-5 h-5 flex-shrink-0" />
                            <p className="text-sm font-medium">{errorMsg}</p>
                        </div>
                    )}

                    <div className="space-y-4">
                        {/* Username Field */}
                        <div className="space-y-2">
                            <label className="block text-primary-dark font-semibold text-sm uppercase tracking-wider ml-1">
                                Логин
                            </label>
                            <div className="relative group">
                                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-primary-dark/40 group-focus-within:text-primary-dark transition-colors">
                                    <User className="w-5 h-5" />
                                </div>
                                <input
                                    type="text"
                                    value={username}
                                    onChange={(e) => setUsername(e.target.value)}
                                    placeholder="Придумайте логин"
                                    className="w-full h-[55px] pl-12 pr-4 rounded-[15px] bg-primary-dark/[0.04] border-2 border-transparent focus:border-primary-dark/10 focus:bg-white transition-all text-lg focus:outline-none"
                                />
                            </div>
                        </div>

                        {/* Password Field */}
                        <div className="space-y-2">
                            <label className="block text-primary-dark font-semibold text-sm uppercase tracking-wider ml-1">
                                Пароль
                            </label>
                            <div className="relative group">
                                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-primary-dark/40 group-focus-within:text-primary-dark transition-colors">
                                    <Lock className="w-5 h-5" />
                                </div>
                                <input
                                    type="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    placeholder="••••••••"
                                    className="w-full h-[55px] pl-12 pr-4 rounded-[15px] bg-primary-dark/[0.04] border-2 border-transparent focus:border-primary-dark/10 focus:bg-white transition-all text-lg focus:outline-none"
                                />
                            </div>
                        </div>

                        {/* Confirm Password Field */}
                        <div className="space-y-2">
                            <label className="block text-primary-dark font-semibold text-sm uppercase tracking-wider ml-1">
                                Подтвердите пароль
                            </label>
                            <div className="relative group">
                                <div className="absolute left-4 top-1/2 -translate-y-1/2 text-primary-dark/40 group-focus-within:text-primary-dark transition-colors">
                                    <Lock className="w-5 h-5" />
                                </div>
                                <input
                                    type="password"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                    placeholder="••••••••"
                                    className="w-full h-[55px] pl-12 pr-4 rounded-[15px] bg-primary-dark/[0.04] border-2 border-transparent focus:border-primary-dark/10 focus:bg-white transition-all text-lg focus:outline-none"
                                />
                            </div>
                        </div>
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full h-[60px] bg-primary-dark text-white rounded-[15px] text-xl font-bold shadow-lg shadow-primary-dark/20 hover:scale-[1.02] active:scale-[0.98] transition-all disabled:opacity-50 mt-4"
                    >
                        {isLoading ? "Регистрация..." : "Зарегистрироваться"}
                    </button>

                    <p className="text-center text-primary-dark/40 text-sm">
                        Уже есть аккаунт?{" "}
                        <span
                            onClick={() => navigate("/login")}
                            className="text-primary-dark font-bold cursor-pointer hover:underline"
                        >
                            Войти
                        </span>
                    </p>
                </form>
            </div>
        </div>
    );
}
