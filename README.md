
# Aplikace pro vizualizaci léčebného procesu

Tento projekt je multiplatformní mobilní a desktopová aplikace vyvinutá v rámci bakalářské práce. Aplikace slouží k přehledné vizualizaci léčebného plánu, umožňuje sledovat jednotlivé kroky léčby, jejich termíny a související události v kalendáři.

Projekt je vyvinut pomocí frameworku **.NET MAUI** a využívá architektonický vzor **MVVM**.

---

## Požadavky

Pro úspěšné sestavení a spuštění projektu je potřeba mít nainstalováno:

- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (doporučená edice: Community, Professional nebo Enterprise)
- Zvolený při instalaci workload: **.NET Multi-platform App UI development (MAUI)**
- .NET SDK 7.0 nebo vyšší
- Volitelně: Android SDK (pokud chcete aplikaci spustit na Android emulátoru)

---

## Otevření projektu

1. Otevřete řešení (`*.sln` soubor) v prostředí Visual Studio 2022.
2. Vyberte cílovou platformu:
   - `Windows Machine` – pro spuštění jako desktopová aplikace
   - `Android Emulator` – pokud chcete testovat na mobilním zařízení/emulátoru
3. Klikněte na **Start** (F5) pro sestavení a spuštění projektu.

---

## Testování na Androidu

Chcete-li aplikaci spustit na Android zařízení nebo emulátoru:

1. V Visual Studiu otevřete **Device Manager** (`Tools > Android > Android Device Manager`).
2. Vytvořte nový emulátor nebo připojte fyzické zařízení.
3. Po úspěšném detekování se zařízení zobrazí v rozbalovacím seznamu vedle tlačítka Start.
4. Vyberte zařízení a spusťte aplikaci.

---

## Struktura projektu

- `Models/` – datové modely (Diagnosis, TreatmentStep, atd.)
- `ViewModels/` – logika aplikace a propojení s View
- `Views/` – jednotlivé stránky aplikace (MainPage, CalendarPage, apod.)
- `Controls/` – vlastní komponenty, např. DeadlineBar
- `Resources/` – styly, barvy, obrázky