package day6

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
	"strings"
)

type LanternFisch struct {
	timer int
}

type LanternFishPopulation struct {
	lanternFishes []LanternFisch
}

func Solve() {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines("day6/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// PartOne
	partOne(inputLines)
}

func partOne(inputLines []string) {
	// Get first generation
	firstGeneration := getFirstGeneration(inputLines[0])
	lanternFischPopulation := LanternFishPopulation{lanternFishes: firstGeneration}

	// fmt.Printf("Initial state:: %s", lanternFischPopulation.String())
	// fmt.Println()
	for day := 0; day < 80; day++ {
		lanternFischPopulation.LetADayPass()
		// fmt.Printf("After %2d day(s): %s", day, lanternFischPopulation.String())
		// fmt.Println()
	}

	answer := lanternFischPopulation.Count()
	fmt.Print("Day 6 - How many lanternfish would there be after 80 days? ")
	fmt.Printf("Answer: [%d]\n", answer)
}

func getFirstGeneration(inputLine string) (firstGeneration []LanternFisch) {
	splits := strings.Split(inputLine, ",")
	for _, timerValueString := range splits {
		timerValue, _ := strconv.Atoi(timerValueString)
		firstGeneration = append(firstGeneration, LanternFisch{timer: timerValue})
	}

	return
}

func (lanternFisch *LanternFisch) LiveADay() (nextGeneration []LanternFisch) {
	if lanternFisch.timer == 0 {
		nextGeneration = append(nextGeneration, LanternFisch{timer: 8})
		lanternFisch.timer = 6
		return
	}

	lanternFisch.timer--
	return
}

func (lanternFishPopulation *LanternFishPopulation) LetADayPass() {
	var overallNextGeneration []LanternFisch
	for i := 0; i < len(lanternFishPopulation.lanternFishes); i++ {
		nextGeneration := lanternFishPopulation.lanternFishes[i].LiveADay()
		for p := 0; p < len(nextGeneration); p++ {
			overallNextGeneration = append(overallNextGeneration, nextGeneration[p])
		}
	}

	for p := 0; p < len(overallNextGeneration); p++ {
		lanternFishPopulation.lanternFishes = append(lanternFishPopulation.lanternFishes, overallNextGeneration[p])
	}
}

func (lanternFishPopulation *LanternFishPopulation) String() string {
	var timerValues []string
	for i := 0; i < len(lanternFishPopulation.lanternFishes); i++ {
		timerValues = append(timerValues, fmt.Sprintf("%d", lanternFishPopulation.lanternFishes[i].timer))
	}

	return strings.Join(timerValues, ",")
}

func (lanternFishPopulation *LanternFishPopulation) Count() int {
	return len(lanternFishPopulation.lanternFishes)
}
