#pragma once

typedef struct _CONSOLE_FONT {
	DWORD index;
	COORD dim;
} CONSOLE_FONT;

BOOL WINAPI SetConsoleFont(HANDLE hOutput, DWORD fontIndex);
BOOL WINAPI GetConsoleFontInfo(HANDLE hOutput, BOOL bMaximize, DWORD numFonts, CONSOLE_FONT* info);
DWORD WINAPI GetNumberOfConsoleFonts();
BOOL WINAPI SetConsoleIcon(HICON hIcon);

