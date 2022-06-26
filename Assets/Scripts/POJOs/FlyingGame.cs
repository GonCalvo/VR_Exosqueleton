using System;

public class FlyingGame: Game
{
	public int id;
	public int session;
	public float distance_between_targets;
	public int total_targets;
	public float targets_size;
	public int targets_hit;
	public int expected_targets_hit;
	public int maximum_streak;
	public int expected_max_streak;
	public int targets_hit_importance;
	public int max_streak_importance;
	public int calification_targets_hit;
	public int calification_max_streak;
	public int targets_hit_difficulty;
	public int max_streak_difficulty;

	public FlyingGame(int id, int session, float distance_between_targets, int total_targets, float targets_size, int targets_hit,
		int expected_targets_hit, int maximum_streak, int expected_max_streak,int targets_hit_importance, int max_streak_importance, 
		int calification_targets_hit, int calification_max_streak, float GAS_score)
    {
		this.id = id;
		this.session = session;
		this.distance_between_targets = distance_between_targets;
		this.total_targets = total_targets;
		this.targets_size = targets_size;
		this.targets_hit = targets_hit;
		this.expected_targets_hit = expected_targets_hit;
		this.maximum_streak = maximum_streak;
		this.expected_max_streak = expected_max_streak;
		this.GAS_score = GAS_score;
		this.targets_hit_importance = targets_hit_importance;
		this.max_streak_importance = max_streak_importance;
		this.calification_max_streak = calification_max_streak;
		this.calification_targets_hit = calification_targets_hit;
	}

	public FlyingGame(int session, float distance_between_targets, int total_targets, float targets_size,
        int expected_targets_hit, int expected_max_streak, int targets_hit_importance, int max_streak_importance, int targets_hit_difficulty, int max_streak_difficulty)
    {
        this.session = session;
        this.distance_between_targets = distance_between_targets;
        this.total_targets = total_targets;
        this.targets_size = targets_size;
        this.expected_targets_hit = expected_targets_hit;
        this.expected_max_streak = expected_max_streak;
        this.targets_hit_importance = targets_hit_importance * targets_hit_difficulty;
        this.max_streak_importance = max_streak_importance * max_streak_difficulty;
    }

	public double CalculateGASScore()
    {
		double weight_sum_squared = Math.Pow(targets_hit_importance + max_streak_importance, 2);
		GAS_score = (float) (50 + 10*(calification_targets_hit * targets_hit_importance + calification_max_streak * max_streak_importance)/
			Math.Sqrt(0.7*weight_sum_squared + 0.3 * weight_sum_squared ) );
		return GAS_score;
    }

}
