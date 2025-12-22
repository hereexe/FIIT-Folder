import React, { useState } from "react";
import { useLoginMutation } from "../../api/api";
import { useNavigate } from "react-router-dom";
import { ChevronLeft, Lock, User, AlertCircle } from "lucide-react";

export default function Login_page() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMsg, setErrorMsg] = useState("");
    const navigate = useNavigate();

    const [login, { isLoading }] = useLoginMutation();

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrorMsg("");

        if (!username || !password) {
            setErrorMsg("Please enter both username and password");
            return;
        }

        try {
            await login({ username, password }).unwrap();
            sessionStorage.setItem("userName", username)
            navigate("/main_page");
        } catch (err: any) {
            console.error("Login failed:", err);
            setErrorMsg(err.data?.message || "Invalid username or password");
        }
    };

    return (
        <div className="min-h-screen bg-app-bg flex items-center justify-center p-4 font-[Inter]">
            <div className="w-full max-w-[450px] bg-white rounded-[25px] shadow-2xl overflow-hidden">
                {/* Header section with back button */}
                <div className="px-8 pt-8 pb-4">
                    <button
                        onClick={() => navigate(-1)}
                        className="flex items-center gap-2 text-primary-dark hover:opacity-70 transition-opacity mb-8"
                    >
                        <ChevronLeft className="w-6 h-6" />
                        <span className="text-lg font-medium">Назад</span>
                    </button>

                    <h1 className="text-4xl font-bold text-primary-dark tracking-tight mb-2">Войти</h1>
                    <p className="text-primary-dark/60 text-lg">Добро пожаловать в FIIT Folder</p>
                </div>

                {/* Login Form */}
                <form onSubmit={handleLogin} className="px-8 pb-10 space-y-6">
                    {errorMsg && (
                        <div className="flex items-center gap-2 p-4 bg-red-50 text-red-600 rounded-[12px] border border-red-100 animate-in fade-in slide-in-from-top-2 duration-300">
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
                                    placeholder="admin"
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
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full h-[60px] bg-primary-dark text-white rounded-[15px] text-xl font-bold shadow-lg shadow-primary-dark/20 hover:scale-[1.02] active:scale-[0.98] transition-all disabled:opacity-50 disabled:hover:scale-100 mt-4"
                    >
                        {isLoading ? (
                            <div className="flex items-center justify-center gap-2">
                                <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin" />
                                <span>Загрузка...</span>
                            </div>
                        ) : (
                            "Войти"
                        )}
                    </button>

                    <p className="text-center text-primary-dark/40 text-sm">
                        Нет аккаунта?{" "}
                        <span
                            onClick={() => navigate("/register")}
                            className="text-primary-dark font-bold cursor-pointer hover:underline"
                        >
                            Зарегистрироваться
                        </span>
                    </p>
                </form>
            </div>
        </div>
    );
}
