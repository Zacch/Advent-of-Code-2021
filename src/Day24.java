package se.piro;

import org.chocosolver.solver.Model;
import org.chocosolver.solver.Solver;
import org.chocosolver.solver.variables.BoolVar;
import org.chocosolver.solver.variables.IntVar;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.List;

public class Day24 {

    private static final int linesPerStep = 18;

    public static void Run() throws IOException {

        List<String> lines = Files.readAllLines(Paths.get("Day24.txt"), StandardCharsets.UTF_8);
        var steps = 1 + (lines.size() / linesPerStep);
        int[] divisors = new int[steps];
        int[] xOffsets = new int[steps];
        int[] yOffsets = new int[steps];

        for (int i = 1; i < steps; i++) {
            int offset = (i - 1) * linesPerStep;
            divisors[i] = Integer.parseInt(lines.get(offset + 4).split(" ")[2]);
            xOffsets[i] = Integer.parseInt(lines.get(offset + 5).split(" ")[2]);
            yOffsets[i] = Integer.parseInt(lines.get(offset + 15).split(" ")[2]);
        }
        solve(steps, divisors, xOffsets, yOffsets);
    }

    private static void solve(int steps, int[] divisors, int[] xOffsets, int[] yOffsets) {
        Model model = new Model("Day 24");

        IntVar[] w = model.intVarArray("W", steps, 1, 9, false);
        BoolVar[] x = model.boolVarArray("X", steps);
        IntVar[] y = model.intVarArray("Y", steps, -10000, 10000, false);
        IntVar[] z = model.intVarArray("Z", steps, -1000000, 1000000, false);

        w[0].eq(1).post();
        x[0].eq(0).post();
        y[0].eq(0).post();
        z[0].eq(0).post();

        for (var i = 1; i < steps; i++) {
            x[i].eq(z[i - 1].mod(divisors[i]).add(xOffsets[i]).ne(w[i])).post();
            y[i].eq(x[i].mul(w[i].add(yOffsets[i]))).post();
            z[i].eq(z[i - 1].div(divisors[i]).mul(x[i].mul(25).add(1)).add(x[i].mul(y[i]))).post();
        }

        z[steps - 1].eq(0).post();

        Solver solver = model.getSolver();

        long part1 = 0;
        long part2 = 99999999999999L;
        while (solver.solve()) {
            long result = 0;
            for (int i = 1; i < steps; i++) {
                result = 10 * result + w[i].getValue();
            }
            part1 = Math.max(part1, result);
            part2 = Math.min(part2, result);
        }
        System.out.println("Part 1: " + part1);
        System.out.println("Part 2: " + part2);
    }
}