package day12

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strings"
)

type Cave struct {
	name        string
	connections []*Cave
}

type Path []string

type CaveRoutes struct {
	currentPath                  Path
	noOfPathes                   int
	pathes                       []string
	smallCaveThatCanBeVisitTwice string
}

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	caves := getCaves(inputLines)

	caveRoutes := CaveRoutes{currentPath: make(Path, 0), noOfPathes: 0, pathes: make([]string, 0)}
	startCave := caves["start"]
	startCave.FindPathes(&caveRoutes)

	return caveRoutes.noOfPathes
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	caves := getCaves(inputLines)
	var smallCaveNames []string
	for _, cave := range caves {
		if cave.IsSmallCave() && cave.name != "start" && cave.name != "end" {
			smallCaveNames = append(smallCaveNames, cave.name)
		}
	}

	pathes := make([]string, 0)
	for _, smallCaveName := range smallCaveNames {
		caveRoutes := CaveRoutes{currentPath: make(Path, 0), noOfPathes: 0, pathes: pathes, smallCaveThatCanBeVisitTwice: smallCaveName}
		startCave := caves["start"]
		startCave.FindPathes2(&caveRoutes)
		pathes = caveRoutes.pathes
		// noOfPathes += caveRoutes.noOfPathes
		// for _, path := range caveRoutes.pathes {
		// 	fmt.Println(path)
		// }
		// fmt.Println()
	}

	return len(pathes)
}

func getCaves(intputLines []string) (caves map[string]*Cave) {
	caves = make(map[string]*Cave)
	for _, line := range intputLines {
		splits := strings.Split(line, "-")
		cave1Name := splits[0]
		cave2Name := splits[1]
		if _, ok := caves[cave1Name]; !ok {
			caves[cave1Name] = &Cave{name: cave1Name, connections: make([]*Cave, 0)}
		}

		if _, ok := caves[cave2Name]; !ok {
			caves[cave2Name] = &Cave{name: cave2Name, connections: make([]*Cave, 0)}
		}

		cave1, _ := caves[cave1Name]
		cave2, _ := caves[cave2Name]
		cave1.connections = append(cave1.connections, cave2)
		cave2.connections = append(cave2.connections, cave1)
	}

	return
}

// Cave extensions
func (cave *Cave) IsSmallCave() bool {
	return cave.name == strings.ToLower(cave.name)
}

func (cave *Cave) FindPathes(caveRoutes *CaveRoutes) {
	if cave.IsSmallCave() && caveRoutes.currentPath.ContainesCave(cave) {
		return
	}

	caveRoutes.currentPath = append(caveRoutes.currentPath, cave.name)

	if cave.name == "end" {
		caveRoutes.noOfPathes++
		caveRoutes.currentPath = caveRoutes.currentPath[:len(caveRoutes.currentPath)-1]
		return
	}

	for _, connectedCave := range cave.connections {
		connectedCave.FindPathes(caveRoutes)
	}

	caveRoutes.currentPath = caveRoutes.currentPath[:len(caveRoutes.currentPath)-1]
}

func (cave *Cave) FindPathes2(caveRoutes *CaveRoutes) {
	if cave.IsSmallCave() {
		if cave.name == caveRoutes.smallCaveThatCanBeVisitTwice && caveRoutes.currentPath.CountVisitsAtCave(cave) >= 2 {
			return
		} else if cave.name != caveRoutes.smallCaveThatCanBeVisitTwice && caveRoutes.currentPath.CountVisitsAtCave(cave) >= 1 {
			return
		}
	}

	caveRoutes.currentPath = append(caveRoutes.currentPath, cave.name)

	if cave.name == "end" {
		newPath := strings.Join(caveRoutes.currentPath, ",")
		if !caveRoutes.ContainsPath(newPath) {
			caveRoutes.pathes = append(caveRoutes.pathes, newPath)
			caveRoutes.noOfPathes++
		}

		caveRoutes.currentPath = caveRoutes.currentPath[:len(caveRoutes.currentPath)-1]
		return
	}

	for _, connectedCave := range cave.connections {
		connectedCave.FindPathes2(caveRoutes)
	}

	caveRoutes.currentPath = caveRoutes.currentPath[:len(caveRoutes.currentPath)-1]
}

// Path extension
func (path *Path) ContainesCave(cave *Cave) bool {
	for _, caveName := range *path {
		if caveName == cave.name {
			return true
		}
	}

	return false
}

func (path *Path) CountVisitsAtCave(cave *Cave) (visites int) {
	for _, caveName := range *path {
		if caveName == cave.name {
			visites++
		}
	}

	return
}

// CaveRouts enstensions
func (caveRoutes *CaveRoutes) PrintCurrentPath() {
	fmt.Println(strings.Join(caveRoutes.currentPath, ","))
}

func (caveRoutes *CaveRoutes) ContainsPath(path string) bool {
	for _, pathString := range caveRoutes.pathes {
		if pathString == path {
			return true
		}
	}

	return false
}
