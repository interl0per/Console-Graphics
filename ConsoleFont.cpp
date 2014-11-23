#include <windows.h>
#include <tchar.h>

#include "ConsoleFont.h"

BOOL WINAPI SetConsoleFont(HANDLE hOutput, DWORD fontIndex) {
	typedef BOOL (WINAPI *PSetConsoleFont)(HANDLE, DWORD);
	static PSetConsoleFont pSetConsoleFont = NULL;

	if(pSetConsoleFont == NULL)
		pSetConsoleFont = (PSetConsoleFont)::GetProcAddress(::GetModuleHandle(_T("kernel32")), "SetConsoleFont");
	if(pSetConsoleFont == NULL) return FALSE;

	return pSetConsoleFont(hOutput, fontIndex);
}

BOOL WINAPI GetConsoleFontInfo(HANDLE hOutput, BOOL bMaximize, DWORD fontIndex, CONSOLE_FONT* info) {
	typedef BOOL (WINAPI *PGetConsoleFontInfo)(HANDLE, BOOL, DWORD, CONSOLE_FONT*);
	static PGetConsoleFontInfo pGetConsoleFontInfo = NULL;
	if(pGetConsoleFontInfo == NULL)
		pGetConsoleFontInfo = (PGetConsoleFontInfo)::GetProcAddress(::GetModuleHandle(_T("kernel32")), "GetConsoleFontInfo");
	if(pGetConsoleFontInfo == NULL) return FALSE;

	return pGetConsoleFontInfo(hOutput, bMaximize, fontIndex, info);
}

DWORD WINAPI GetNumberOfConsoleFonts() {
	typedef DWORD (WINAPI *PGetNumberOfConsoleFonts)();
	static PGetNumberOfConsoleFonts pGetNumberOfConsoleFonts = NULL;
	if(pGetNumberOfConsoleFonts == NULL)
		pGetNumberOfConsoleFonts = (PGetNumberOfConsoleFonts)::GetProcAddress(::GetModuleHandle(_T("kernel32")), "GetNumberOfConsoleFonts");
	if(pGetNumberOfConsoleFonts == NULL) return 0;
	return pGetNumberOfConsoleFonts();
}

BOOL WINAPI SetConsoleIcon(HICON hIcon) {
	typedef BOOL (WINAPI *PSetConsoleIcon)(HICON);
	static PSetConsoleIcon pSetConsoleIcon = NULL;
	if(pSetConsoleIcon == NULL)
		pSetConsoleIcon = (PSetConsoleIcon)::GetProcAddress(::GetModuleHandle(_T("kernel32")), "SetConsoleIcon");
	if(pSetConsoleIcon == NULL) return FALSE;
	return pSetConsoleIcon(hIcon);
}
