using System.ComponentModel.DataAnnotations;
using LinqConsoleLab.PL.Data;
using LinqConsoleLab.PL.Models;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{
    /// <summary>
    /// Zadanie:
    /// Wyszukaj wszystkich studentów mieszkających w Warsaw.
    /// Zwróć numer indeksu, pełne imię i nazwisko oraz miasto.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko, Miasto
    /// FROM Studenci
    /// WHERE Miasto = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        var result = from student in DaneUczelni.Studenci
            where student.Miasto.Equals("Warsaw")
                select $"{student.NumerIndeksu}, {student.Imie}, {student.Nazwisko}, {student.Miasto}";
        
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj listę adresów e-mail wszystkich studentów.
    /// Użyj projekcji, tak aby w wyniku nie zwracać całych obiektów.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Studenci;
    /// </summary>
    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        var result = DaneUczelni.Studenci.Select(student => student.Email);
        
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Posortuj studentów alfabetycznie po nazwisku, a następnie po imieniu.
    /// Zwróć numer indeksu i pełne imię i nazwisko.
    ///
    /// SQL:
    /// SELECT NumerIndeksu, Imie, Nazwisko
    /// FROM Studenci
    /// ORDER BY Nazwisko, Imie;
    /// </summary>
    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        var result = from student in DaneUczelni.Studenci
            orderby student.Nazwisko, student.Imie
                select $"{student.NumerIndeksu}, {student.Imie}, {student.Nazwisko}";
        
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Znajdź pierwszy przedmiot z kategorii Analytics.
    /// Jeżeli taki przedmiot nie istnieje, zwróć komunikat tekstowy.
    ///
    /// SQL:
    /// SELECT TOP 1 Nazwa, DataStartu
    /// FROM Przedmioty
    /// WHERE Kategoria = 'Analytics';
    /// </summary>
    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var result = "";
        try
        {
            result = (from przedmiot in DaneUczelni.Przedmioty
                where przedmiot.Kategoria.Equals("Analytics")
                select $"{przedmiot.Nazwa}, {przedmiot.DataStartu}").First();

            if (result.Equals("")) throw new InvalidOperationException();
        } catch (InvalidOperationException) { return ["Nie ma takiego przedmiotu"]; }

        return [result];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy w danych istnieje przynajmniej jeden nieaktywny zapis.
    /// Zwróć jedno zdanie z odpowiedzią True/False albo Tak/Nie.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Zapisy
    ///     WHERE CzyAktywny = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var result = from zapis in DaneUczelni.Zapisy
            where(zapis.CzyAktywny == false)
                select zapis;
        if (!result.Any()) return ["Nie"];
        
        return ["Tak"];
    }

    /// <summary>
    /// Zadanie:
    /// Sprawdź, czy każdy prowadzący ma uzupełnioną nazwę katedry.
    /// Warto użyć metody, która weryfikuje warunek dla całej kolekcji.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Katedra)
    /// THEN 1 ELSE 0 END
    /// FROM Prowadzacy;
    /// </summary>
    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var result = DaneUczelni.Prowadzacy.All(prowadzacy => prowadzacy.Katedra != null && prowadzacy.Katedra != "");
    
        return result ? ["Tak"] : ["Nie"];
    }

    /// <summary>
    /// Zadanie:
    /// Policz, ile aktywnych zapisów znajduje się w systemie.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Zapisy
    /// WHERE CzyAktywny = 1;
    /// </summary>
    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var result = DaneUczelni.Zapisy.Where(zapis => zapis.CzyAktywny).Count();
        
        return [$"{result}"];
    }

    /// <summary>
    /// Zadanie:
    /// Pobierz listę unikalnych miast studentów i posortuj ją rosnąco.
    ///
    /// SQL:
    /// SELECT DISTINCT Miasto
    /// FROM Studenci
    /// ORDER BY Miasto;
    /// </summary>
    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        var result = DaneUczelni.Studenci.Select(student => student.Miasto).Distinct().OrderBy(student => student);
        
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zwróć trzy najnowsze zapisy na przedmioty.
    /// W wyniku pokaż datę zapisu, identyfikator studenta i identyfikator przedmiotu.
    ///
    /// SQL:
    /// SELECT TOP 3 DataZapisu, StudentId, PrzedmiotId
    /// FROM Zapisy
    /// ORDER BY DataZapisu DESC;
    /// </summary>
    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        var result = DaneUczelni.Zapisy.OrderByDescending(zapis => zapis.DataZapisu)
            .Select(zapis => $"{zapis.DataZapisu}, {zapis.StudentId}, {zapis.PrzedmiotId}").Take(3);
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Zaimplementuj prostą paginację dla listy przedmiotów.
    /// Załóż stronę o rozmiarze 2 i zwróć drugą stronę danych.
    ///
    /// SQL:
    /// SELECT Nazwa, Kategoria
    /// FROM Przedmioty
    /// ORDER BY Nazwa
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        var result = DaneUczelni.Przedmioty.OrderBy(przedmiot => przedmiot.Nazwa).Skip(2).Take(2)
            .Select(przedmiot => $"{przedmiot.Nazwa}, {przedmiot.Kategoria}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Połącz studentów z zapisami po StudentId.
    /// Zwróć pełne imię i nazwisko studenta oraz datę zapisu.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, z.DataZapisu
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId;
    /// </summary>
    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        var result = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id, zapis => zapis.StudentId,
                (student, zapis) => new { student.Imie, student.Nazwisko, zapis.DataZapisu })
            .Select(joined => $"{joined.Imie}, {joined.Nazwisko}, {joined.DataZapisu}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Przygotuj wszystkie pary student-przedmiot na podstawie zapisów.
    /// Użyj podejścia, które pozwoli spłaszczyć dane do jednej sekwencji wyników.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, p.Nazwa
    /// FROM Zapisy z
    /// JOIN Studenci s ON s.Id = z.StudentId
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId;
    /// </summary>
    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        var result = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy, student => student.Id, zapis => zapis.StudentId,
                (student, zapis) => new { student.Id, student.Imie, student.Nazwisko, zapis.PrzedmiotId })
            .Join(DaneUczelni.Przedmioty, joined => joined.PrzedmiotId, przedmiot => przedmiot.Id,
                (joined, przedmiot) => new { joined.Imie, joined.Nazwisko, przedmiot.Nazwa })
            .Select(twjoined => $"{twjoined.Imie}, {twjoined.Nazwisko}, {twjoined.Nazwa}");
        
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Pogrupuj zapisy według przedmiotu i zwróć nazwę przedmiotu oraz liczbę zapisów.
    ///
    /// SQL:
    /// SELECT p.Nazwa, COUNT(*)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        var result = DaneUczelni.Zapisy.Join(DaneUczelni.Przedmioty, zapis => zapis.PrzedmiotId, przedmiot => przedmiot.Id, (zapis, przedmiot) => new { przedmiot.Nazwa })
            .GroupBy(joined => joined.Nazwa)
            .Select(group => $"{group.Key}, {group.Count()}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Oblicz średnią ocenę końcową dla każdego przedmiotu.
    /// Pomiń rekordy, w których ocena końcowa ma wartość null.
    ///
    /// SQL:
    /// SELECT p.Nazwa, AVG(z.OcenaKoncowa)
    /// FROM Zapisy z
    /// JOIN Przedmioty p ON p.Id = z.PrzedmiotId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY p.Nazwa;
    /// </summary>
    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        var result = DaneUczelni.Zapisy.Where(zapis => zapis.OcenaKoncowa != null).Join(DaneUczelni.Przedmioty,
            zapis => zapis.PrzedmiotId, przedmiot => przedmiot.Id,
            (zapis, przedmiot) => new { przedmiot.Nazwa, zapis.OcenaKoncowa })
            .GroupBy( joined => joined.Nazwa)
            .Select(group => $"{group.Key}, {group.Average(zapis => zapis.OcenaKoncowa)}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego prowadzącego policz liczbę przypisanych przedmiotów.
    /// W wyniku zwróć pełne imię i nazwisko oraz liczbę przedmiotów.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, COUNT(p.Id)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        var result = DaneUczelni.Prowadzacy.GroupJoin(DaneUczelni.Przedmioty, prowadzacy => prowadzacy.Id,
                przedmiot => przedmiot.ProwadzacyId,
                (prowadzacy, przedmioty) => new { prowadzacy.Imie, prowadzacy.Nazwisko, przedmioty })
            .Select(group => $"{group.Imie} {group.Nazwisko}, {group.przedmioty.Count()}");
        return result;
    }

    /// <summary>
    /// Zadanie:
    /// Dla każdego studenta znajdź jego najwyższą ocenę końcową.
    /// Pomiń studentów, którzy nie mają jeszcze żadnej oceny.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, MAX(z.OcenaKoncowa)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY s.Imie, s.Nazwisko;
    /// </summary>
    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        var result = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy.Where(zapis => zapis.OcenaKoncowa != null), student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new { student.Imie, student.Nazwisko, zapis.OcenaKoncowa })
            .GroupBy(joined => $"{joined.Imie} {joined.Nazwisko}")
            .Select(group => $"{group.Key}, {group.Max(zapis => zapis.OcenaKoncowa)}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Znajdź studentów, którzy mają więcej niż jeden aktywny zapis.
    /// Zwróć pełne imię i nazwisko oraz liczbę aktywnych przedmiotów.
    ///
    /// SQL:
    /// SELECT s.Imie, s.Nazwisko, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Imie, s.Nazwisko
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        var result = DaneUczelni.Studenci.Join(DaneUczelni.Zapisy.Where(zapis => zapis.CzyAktywny), student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new { student.Imie, student.Nazwisko })
            .GroupBy(joined => $"{joined.Imie} {joined.Nazwisko}")
            .Where(group => group.Count() > 1)
            .Select(group => $"{group.Key}, {group.Count()}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Wypisz przedmioty startujące w kwietniu 2026, dla których żaden zapis nie ma jeszcze oceny końcowej.
    ///
    /// SQL:
    /// SELECT p.Nazwa
    /// FROM Przedmioty p
    /// JOIN Zapisy z ON p.Id = z.PrzedmiotId
    /// WHERE MONTH(p.DataStartu) = 4 AND YEAR(p.DataStartu) = 2026
    /// GROUP BY p.Nazwa
    /// HAVING SUM(CASE WHEN z.OcenaKoncowa IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        var result = DaneUczelni.Przedmioty.Where(przedmiot => przedmiot.DataStartu.Month == 4 && przedmiot.DataStartu.Year == 2026)
            .Join(DaneUczelni.Zapisy, przedmiot => przedmiot.Id, zapis => zapis.PrzedmiotId, (przedmiot, zapis) => new {przedmiot.Nazwa, zapis.OcenaKoncowa } )
            .GroupBy(group => group.Nazwa)
            .Where(group => group.All(g => g.OcenaKoncowa == null))
            .Select(group => $"{group.Key}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Oblicz średnią ocen końcowych dla każdego prowadzącego na podstawie wszystkich jego przedmiotów.
    /// Pomiń brakujące oceny, ale pozostaw samych prowadzących w wyniku.
    ///
    /// SQL:
    /// SELECT pr.Imie, pr.Nazwisko, AVG(z.OcenaKoncowa)
    /// FROM Prowadzacy pr
    /// LEFT JOIN Przedmioty p ON p.ProwadzacyId = pr.Id
    /// LEFT JOIN Zapisy z ON z.PrzedmiotId = p.Id
    /// WHERE z.OcenaKoncowa IS NOT NULL
    /// GROUP BY pr.Imie, pr.Nazwisko;
    /// </summary>
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        var result = DaneUczelni.Prowadzacy
            .Join(DaneUczelni.Przedmioty, prowadzacy => prowadzacy.Id, przedmiot => przedmiot.ProwadzacyId, (prowadzacy, przedmiot) => new { prowadzacy.Imie, prowadzacy.Nazwisko, przedmiot.Id })
            .Join(DaneUczelni.Zapisy.Where(zapis => zapis.OcenaKoncowa != null), joined => joined.Id, zapis => zapis.PrzedmiotId, (joined, zapis) => new { joined.Imie, joined.Nazwisko, zapis.OcenaKoncowa })
            .GroupBy(joined => $"{joined.Imie} {joined.Nazwisko}")
            .Select(group => $"{group.Key}, {group.Average(zapis => zapis.OcenaKoncowa)}");
        return result;
    }

    /// <summary>
    /// Wyzwanie:
    /// Pokaż miasta studentów oraz liczbę aktywnych zapisów wykonanych przez studentów z danego miasta.
    /// Posortuj wynik malejąco po liczbie aktywnych zapisów.
    ///
    /// SQL:
    /// SELECT s.Miasto, COUNT(*)
    /// FROM Studenci s
    /// JOIN Zapisy z ON s.Id = z.StudentId
    /// WHERE z.CzyAktywny = 1
    /// GROUP BY s.Miasto
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        var result = DaneUczelni.Studenci
            .Join(DaneUczelni.Zapisy.Where(zapis => zapis.CzyAktywny), student => student.Id, zapis => zapis.StudentId,
                (student, zapis) => new { student.Miasto })
            .GroupBy(joined => joined.Miasto)
            .OrderByDescending(group => group.Count())
            .Select(group => $"{group.Key}, {group.Count()}");
        return result;
    }

    private static NotImplementedException Niezaimplementowano(string nazwaMetody)
    {
        return new NotImplementedException(
            $"Uzupełnij metodę {nazwaMetody} w pliku Exercises/ZadaniaLinq.cs i uruchom polecenie ponownie.");
    }
}
